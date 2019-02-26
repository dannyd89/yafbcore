using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using YAFBCore.Controllables.Commands;
using YAFBCore.Mapping;
using YAFBCore.Mapping.Units;
using YAFBCore.Networking;
using YAFBCore.Pathfinding.Pathfinders;

namespace YAFBCore.Controllables
{
    public class Ship : Controllable
    {
        /// <summary>
        /// Flattiverse ship
        /// </summary>
        private Flattiverse.Ship ship;

        /// <summary>
        /// Current active map
        /// </summary>
        private Map currentMap;

        /// <summary>
        /// 
        /// </summary>
        private long currentMapId = -1;

        /// <summary>
        /// Current active map unit of this ship
        /// </summary>
        private PlayerShipMapUnit playerShipMapUnit;

        #region Scanning Fields
        /// <summary>
        /// Current degree the scanner is at
        /// </summary>
        private float currentScanDegree;

        /// <summary>
        /// Scan reference is a position of a still unit which is used as a reference point for the next scan
        /// This is used to always have at least one
        /// </summary>
        private Flattiverse.Vector scanReference;

        /// <summary>
        /// Waiter which can be used to know if this ship has successfully performed its scan
        /// </summary>
        internal ManualResetEventSlim ScanWaiter = new ManualResetEventSlim(false);
        #endregion

        #region Moving Fields
        /// <summary>
        /// Waiter which can be used to know if this ship has successfully performed its move
        /// </summary>
        internal ManualResetEventSlim MoveWaiter = new ManualResetEventSlim(false);
        
        /// <summary>
        /// Sync object to lock moveCommands queue
        /// </summary>
        private object syncMoveCommands = new object();

        /// <summary>
        /// Stores the issued move commands
        /// </summary>
        private Queue<MoveCommand> userMoveCommands = new Queue<MoveCommand>();

        /// <summary>
        /// Last used move command
        /// </summary>
        private MoveCommand lastMoveCommand;

        /// <summary>
        /// 
        /// </summary>
        public Flattiverse.Vector DesiredPosition => lastMoveCommand?.Position;

        /// <summary>
        /// 
        /// </summary>
        public MapPathfinder MapPathfinder;

        /// <summary>
        /// 
        /// </summary>
        public LinkedList<MoveCommand> pathfindingMoveCommands;

        /// <summary>
        /// Last movement vector
        /// </summary>
        private Flattiverse.Vector movement = Flattiverse.Vector.FromNull();

        public Flattiverse.Vector Movement => movement;
        #endregion

        #region Shooting Fields
        /// <summary>
        /// Waiter which can be used to know if this ship has successfully performed its shots
        /// </summary>
        internal ManualResetEventSlim ShootWaiter = new ManualResetEventSlim(false);

        /// <summary>
        /// Sync object to lock userShootCommands queue
        /// </summary>
        private object syncShootCommands = new object();

        /// <summary>
        /// Stores the issued move commands
        /// </summary>
        private Queue<ShootCommand> userShootCommands = new Queue<ShootCommand>();
        #endregion

        #region Properties
        /// <summary>
        /// Returns the current universe the ship is in
        /// </summary>
        public Flattiverse.Universe Universe => ship.Universe;
        #endregion

        #region Events
        /// <summary>
        /// Is called when ship isActive state is set to false
        /// We want to reregister this ship then in the manager
        /// </summary>
        public event EventHandler ActiveStateChanged;
        #endregion

        /// <summary>
        /// Creates a ship controllable
        /// </summary>
        /// <param name="ship"></param>
        internal Ship(UniverseSession universeSession, Flattiverse.Ship ship) 
            : base(universeSession, ship)
        {
            this.ship = ship;
        }

        /// <summary>
        /// Gets fired if any map was updated
        /// Uses the map where this unit is in
        /// </summary>
        /// <param name="map"></param>
        internal void Map_MapUpdated(Map map)
        {
            if (map.Universe.Name == ship.Universe.Name
                && map.TryGetPlayerShip(ship.Name, out playerShipMapUnit))
                currentMap = map;
        }

        /// <summary>
        /// Reset ship after death
        /// </summary>
        private void reset()
        {
            //lock (syncMoveCommands)
            //    userMoveCommands.Clear();

            //scanReference = null;
            //currentMap = null;
            lastMoveCommand = null;
            //pathfindingMoveCommands = null;

            //movement = Flattiverse.Vector.FromNull();

            resetWaiters();
        }

        /// <summary>
        /// Resets all the waiters of this ship
        /// </summary>
        private void resetWaiters()
        {
            ScanWaiter.Reset();
            MoveWaiter.Reset();
            ShootWaiter.Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void worker()
        {
            flowControl = Session.CreateFlowControl();

            while (!isDisposed)
            {
                try
                {
                    // Wait for the game to calculate the next tick
                    flowControl.FlowControl.PreWait();

                    if (!ship.IsAlive)
                        TryContinue();

                    // Scan and wait
                    scan();

                    // Let the map manager process all the scanned info
                    Session.MapManager.WaitMerge();

                    if (playerShipMapUnit != null)
                    {
                        if (currentMapId != currentMap.Id)
                        {
                            Console.WriteLine("Current map id: " + currentMap.Id);
                            currentMapId = currentMap.Id;
                        }

                        // Perform any shoot command if available
                        shoot();

                        // Perform any move command if available
                        move();
                    }

                    // Commit actions queued by this tick
                    flowControl.FlowControl.Commit();

                    // Reset all waiters
                    resetWaiters();
                }
                catch (Exception ex)
                {
                    if (isDisposed)
                        return;

                    Debug.WriteLine($"{ship.Name}: Worker Exception");
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Performs a scan with the ship
        /// </summary>
        protected override void scan()
        {
            try
            {
                List<Flattiverse.ScanInfo> scanInfos = new List<Flattiverse.ScanInfo>();

                int scannerCount = ship.ScannerCount;

                if (scanReference != null)
                {
                    // We got a reference point so we scan in that direction with a minimal degree because we know the direction
                    float oneFourthDegree = Math.Min(10f, ship.ScannerDegreePerScan / 4f);

                    scanInfos.Add(new Flattiverse.ScanInfo(scanReference.Angle - oneFourthDegree, 
                                                           scanReference.Angle + oneFourthDegree, 
                                                           ship.ScannerArea.Limit * 0.99f));

                    --scannerCount;
                }
                
                for (int i = 0; i < scannerCount; i++)
                {
                    Flattiverse.ScanInfo scanInfo = new Flattiverse.ScanInfo(currentScanDegree, currentScanDegree + ship.ScannerDegreePerScan, ship.ScannerArea.Limit * 0.99f);

                    currentScanDegree += ship.ScannerDegreePerScan;

                    if (currentScanDegree >= 360f)
                        currentScanDegree -= 360f;

                    scanInfos.Add(scanInfo);
                }

                List<Flattiverse.Unit> scannedUnits = ship.Scan(scanInfos);

                scanReference = null;

                // Get the nearest still unit as reference point
                for (int i = 0; i < scannedUnits.Count; i++)
                    if (scannedUnits[i].Mobility == Flattiverse.Mobility.Still
                        && scannedUnits[i].Kind != Flattiverse.UnitKind.Explosion
                        && (scanReference == null || scannedUnits[i].Position.Length < scanReference.Length))
                        scanReference = scannedUnits[i].Position;

                Map map = Map.Create(this, scannedUnits);
                map.Updated += Map_MapUpdated;

                Session.MapManager.Add(map);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ship.Name}: Scan Exception");
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);

                TryContinue();
            }
            finally
            {
                ScanWaiter.Set();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void move()
        {
            try
            {
                if (lastMoveCommand != null && (lastMoveCommand.Position - playerShipMapUnit.PositionInternal) < NeededTileSize)
                        lastMoveCommand.Reached = true;

                lock (syncMoveCommands)
                {
                    if (userMoveCommands.Count > 0)
                    {
                        lastMoveCommand = userMoveCommands.Dequeue();

                        // TODO: Wäre vielleicht besser das direkt in der move Funktion zu machen, dass nicht immer ein neuer Pathfinder erzeugt wird
                        //MapPathfinder = currentMap.GetPathFinder(NeededTileSize);
                        //pathfindingMoveCommands = MapPathfinder.Pathfind(playerShipMapUnit.Position, lastMoveCommand.Position);

                        //lastMoveCommand = null;
                    }

                    if ((lastMoveCommand == null || lastMoveCommand.Reached) && pathfindingMoveCommands != null && pathfindingMoveCommands.Count > 0)
                    {
                        lastMoveCommand = pathfindingMoveCommands.First.Value;
                        pathfindingMoveCommands.RemoveFirst();
                    }
                }

                if (lastMoveCommand == null)
                    lastMoveCommand = new MoveCommand(playerShipMapUnit.PositionInternal.X, playerShipMapUnit.PositionInternal.Y);

                movement = lastMoveCommand.Position - playerShipMapUnit.PositionInternal;

                if (movement < 250f)
                {
                    movement.Length = ship.EngineAcceleration.Limit * movement.Length;

                    movement = movement - playerShipMapUnit.MovementInternal;

                    if (movement > ship.EngineAcceleration.Limit * 0.99f)
                        movement.Length = ship.EngineAcceleration.Limit * 0.99f;
                }
                else
                    movement.Length = ship.EngineAcceleration.Limit * 0.99f;

                ship.Move(movement);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ship.Name}: Move Exception");
                Debug.WriteLine(ex.Message);

                userMoveCommands.Clear();
                lastMoveCommand = null;
                pathfindingMoveCommands = null;
            }
            finally
            {
                MoveWaiter.Set();
            }
        }

        /// <summary>
        /// Shoots with the ship
        /// </summary>
        protected override void shoot()
        {
            try
            {
                lock (syncShootCommands)
                {
                    if (userShootCommands.Count > 0)
                        for (int i = 0; i < ship.WeaponProductionStatus && userShootCommands.Count > 0; i++)
                        {
                            ShootCommand shootCommand = userShootCommands.Dequeue();
                            shootCommand.TempSetup(ship, playerShipMapUnit);

                            ship.Shoot(shootCommand.Direction,
                                   shootCommand.Direction.Angle,
                                   shootCommand.Time,
                                   shootCommand.Load,
                                   shootCommand.DamageHull,
                                   shootCommand.DamageShield,
                                   shootCommand.DamageEnergy,
                                   shootCommand.SubDirections);
                        }

                    //if (shootCommand != null)
                    //{

                    //}
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ship.Name}: Shoot Exception");
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                ShootWaiter.Set();
            }
        }

        /// <summary>
        /// Disposes the ship
        /// </summary>
        public override void Dispose()
        {
            try
            {
                resetWaiters();

                ScanWaiter.Dispose();
                MoveWaiter.Dispose();
                ShootWaiter.Dispose();

                // We try to close the ship if it's still active
                ship.Close();

                base.Dispose();

                Debug.WriteLine("Ship disposed!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Continues the ship if possible
        /// <para>Can trigger ActiveStateChanged event if IsActive is false</para>
        /// </summary>
        /// <returns>True if continue was successful</returns>
        public bool TryContinue()
        {
            try
            {
                reset();

                if (!ship.IsActive)
                {
                    ActiveStateChanged?.Invoke(this, EventArgs.Empty);
                    return false;
                }

                if (!ship.IsAlive)
                    ship.Continue();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return false;
            }
        }

        /// <summary>
        /// Queues different commands into this ship and will be handled afterwards
        /// </summary>
        /// <param name="command"></param>
        public override void Queue(Command command)
        {
            switch (command)
            {
                case MoveCommand moveCommand:
                    lock (syncMoveCommands)
                        userMoveCommands.Enqueue(moveCommand);
                    break;
                case ShootCommand shootCommand:
                    lock (syncShootCommands)
                        userShootCommands.Enqueue(shootCommand);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
