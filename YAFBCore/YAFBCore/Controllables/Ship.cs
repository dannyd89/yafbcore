using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using YAFBCore.Controllables.Commands;
using YAFBCore.Mapping.Units;
using YAFBCore.Networking;

namespace YAFBCore.Controllables
{
    public class Ship : Controllable
    {
        #region Events
        /// <summary>
        /// Is called when ship isActive state is set to false
        /// We want to reregister this ship then in the manager
        /// </summary>
        public event EventHandler ActiveStateChanged;
        #endregion

        /// <summary>
        /// Flattiverse ship
        /// </summary>
        private Flattiverse.Ship ship;

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
        private Queue<MoveCommand> moveCommands = new Queue<MoveCommand>();

        /// <summary>
        /// Last used move command
        /// </summary>
        private MoveCommand lastMoveCommand;

        public Flattiverse.Vector DesiredPosition => lastMoveCommand?.Position;

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
        #endregion

        #region Properties
        /// <summary>
        /// Returns the current universe the ship is in
        /// </summary>
        public Flattiverse.Universe Universe => ship.Universe;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ship"></param>
        internal Ship(UniverseSession universeSession, Flattiverse.Ship ship) 
            : base(universeSession, ship)
        {
            this.ship = ship;
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
                if (!ship.IsActive)
                {
                    ActiveStateChanged?.Invoke(this, EventArgs.Empty);
                    return false;
                }

                if (!ship.IsAlive)
                {
                    reset();

                    ship.Continue();
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return false;
            }
        }

        /// <summary>
        /// Reset ship after death
        /// </summary>
        private void reset()
        {
            lastMoveCommand = null;

            resetWaiters();
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

                    // Scan and wait
                    scan();

                    // Let the map manager process all the scanned info
                    Session.MapManager.Wait();

                    // Perform any move command if available
                    move();

                    // Perform any shoot command if available
                    shoot();

                    // Commit actions queued this tick
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
        /// Resets all the waiters of this ship
        /// </summary>
        private void resetWaiters()
        {
            ScanWaiter.Reset();
            MoveWaiter.Reset();
            ShootWaiter.Reset();
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

                //for (int i = 0; i < scannedUnits.Count; i++)
                //    if (scannedUnits[i].Mobility == Flattiverse.Mobility.Still
                //        && scannedUnits[i].Kind != Flattiverse.UnitKind.Explosion)
                //        movement = scannedUnits[i].Movement;

                Session.MapManager.Add(Mapping.Map.Create(ship, scannedUnits));
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
                lock (syncMoveCommands)
                    if (moveCommands.Count > 0)
                        lastMoveCommand = moveCommands.Dequeue();
                
                PlayerShipMapUnit shipUnit;
                if (Session.MapManager.TryGetPlayerUnit(ship.Universe.Name, ship.Name, out shipUnit))
                {
                    if (lastMoveCommand == null)
                        lastMoveCommand = new MoveCommand(shipUnit.PositionInternal.X, shipUnit.PositionInternal.Y);

                    movement = lastMoveCommand.Position - shipUnit.PositionInternal;

                    if (movement < 250f)
                    {
                        movement.Length = ship.EngineAcceleration.Limit * movement.Length;

                        movement = movement - shipUnit.MovementInternal;

                        if (movement > ship.EngineAcceleration.Limit * 0.99f)
                            movement.Length = ship.EngineAcceleration.Limit * 0.99f;
                    }
                    else
                        movement.Length = ship.EngineAcceleration.Limit * 0.99f;

                    ship.Move(movement);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ship.Name}: Move Exception");
                Debug.WriteLine(ex.Message);
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
                base.Dispose();

                ScanWaiter.Dispose();

                flowControl.Dispose();

                // We try to close the ship if it's still active
                ship.Close();

                Debug.WriteLine("Ship disposed!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
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
                        moveCommands.Enqueue(moveCommand);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
