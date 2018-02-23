using FlattiBase.Commands;
using FlattiBase.Mapping;
using FlattiBase.Mapping.MapUnits;
using FlattiBase.Screens;
using Flattiverse;
using JARVIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlattiBase.Ships
{
    public class ShipBase
    {
        #region Universe field
        private readonly UniverseGroup universeGroup;

        public UniverseGroup UniverseGroup
        {
            get { return universeGroup; }
        }
        #endregion

        #region Ship fields
        private Ship ship;
        public Ship Ship
        {
            get { return ship; }
        }

        public bool IsActive
        {
            get
            {
                if (ship == null)
                    return false;

                return ship.IsActive;
            }
        }

        public bool IsAlive
        {
            get 
            {
                if (ship == null)
                    return false;

                return ship.IsAlive;
            }
        }
        #endregion

        #region Screen field
        private readonly GameScreen gameScreen;

        public Screen Screen
        {
            get { return gameScreen; }
        }
        #endregion

        #region Map fields
        private Map shipMap;

        protected Map ShipMap
        {
            get { return shipMap; }
            set { shipMap = value; }
        }

        public List<MapUnit> MapUnits
        {
            get 
            {
                if (shipMap == null)
                    if (gameScreen.GlobalMap != null)
                        return gameScreen.GlobalMap.MapUnits;
                    else
                        return null;

                return shipMap.MapUnits;
            }
        }
        #endregion

        private string shipName;

        private Thread shipThread;

        private BinaryMessenger binaryMessenger;
        private List<Unit> lastSeenUnits;
        private Unit referencingScanUnit;
        private Vector desiredMovePosition = new Vector(0f, 0f);

        private volatile float scanDirection = 0f;
        public volatile bool LoadShield = false;
        public volatile bool RepairSelf = false;
        public volatile bool IsRunning = true;

        private object sync = new object();
        private object syncShootCommands = new object();

        private List<ShootCommand> shootCommands = new List<ShootCommand>();

        public ShipBase(GameScreen gameScreen, UniverseGroup universeGroup, string shipName, BinaryMessenger binaryMessenger)
        {
            this.gameScreen = gameScreen;
            this.universeGroup = universeGroup;
            this.shipName = shipName;
            this.binaryMessenger = binaryMessenger;

            shipThread = new Thread(ShipWorker);
        }

        public void CreateShip(string @class)
        {
            ship = universeGroup.RegisterShip(@class, shipName);

            shipThread.Start();
        }

        public void SendPosition(BinaryMessenger binaryMessenger)
        {
            if (binaryMessenger != null && lastSeenUnits != null)
                binaryMessenger.SendPositionSelf(shipName, ship.Radius, Vector.FromNull(), lastSeenUnits);
        }

        private void ShipWorker()
        {
            //DateTime now = DateTime.Now;

            //int tickCount = 0;

            using (UniverseGroupFlowControl fc = universeGroup.GetNewFlowControl())
            {
                while (IsRunning)
                {
                    fc.PreWait();

                    try
                    {
                        if (ship != null && ship.IsAlive)
                        {
                            #region Benchmark
                            //Console.WriteLine((DateTime.Now - now).ToString() + " Tick / Tack. Mine: " + tickCount.ToString() + " Server: " + uni.Connector.Account.AverageCommitTime.ToString());

                            //System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
                            #endregion

                            Scan();

                            #region Benchmark
                            //if (sw.ElapsedMilliseconds > 60)
                            //    Console.WriteLine("Took me: Scan: " + sw.Elapsed.ToString());
                            #endregion

                            fc.Wait();

                            #region Benchmark
                            //System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
                            #endregion

                            #region Binary Messenger
                            if (binaryMessenger != null && lastSeenUnits != null)
                                binaryMessenger.SendPositionSelf(shipName, ship.Radius, Vector.FromNull(), lastSeenUnits);


                            if (binaryMessenger != null && lastSeenUnits != null)
                                binaryMessenger.SendPositionEnemies(lastSeenUnits);
                            #endregion

                            #region Benchmark
                            //sw.Stop();

                            //Console.WriteLine("Send time: " + sw.Elapsed.ToString());
                            #endregion

                            Move();

                            Shoot();

                            #region Load Repair Benchmarks
                            //if (sw.ElapsedMilliseconds > 60)
                            //    Console.WriteLine("Took me: Move: " + sw.Elapsed.ToString());

                            LoadShields();

                            //Console.WriteLine("Took me: LoadShields: " + sw.Elapsed.ToString());

                            Repair();

                            //Console.WriteLine("Took me: Repair: " + sw.Elapsed.ToString());

                            //sw.Stop();

                            //if (sw.ElapsedMilliseconds > 60)
                            //    Console.WriteLine("Took me: " + sw.Elapsed.ToString());
                            #endregion
                        }
                    }
                    catch
                    {
                        fc.Commit();
                        continue;
                    }

                    fc.Commit();
                }
            }
        }

        /// <summary>
        /// Scan units
        /// </summary>
        private void Scan()
        {
            try
            {
                if (shipMap != null)
                    shipMap.Age();

                List<ScanInfo> scanInfos = new List<ScanInfo>();

                if (referencingScanUnit == null)
                {
                    for (int i = 0; i < ship.ScannerCount; i++)
                    {
                        ScanInfo scanInfo = new ScanInfo(scanDirection, scanDirection + ship.ScannerDegreePerScan, ship.ScannerArea.Limit * 0.99f);

                        scanDirection += ship.ScannerDegreePerScan;

                        if (scanDirection >= 360f)
                            scanDirection -= 360f;

                        scanInfos.Add(scanInfo);
                    }
                }
                else
                {
                    float oneFourthDegree = Math.Min(15f, (ship.ScannerDegreePerScan / 4f));

                    scanInfos.Add(new ScanInfo(referencingScanUnit.Position.Angle - oneFourthDegree - 1f, referencingScanUnit.Position.Angle + oneFourthDegree - 1f, ship.ScannerArea.Limit * 0.99f));

                    for (int i = 0; i < ship.ScannerCount - 1; i++)
                    {
                        ScanInfo scanInfo = new ScanInfo(scanDirection, scanDirection + ship.ScannerDegreePerScan, ship.ScannerArea.Limit * 0.99f);

                        scanDirection += ship.ScannerDegreePerScan;

                        if (scanDirection >= 360f)
                            scanDirection -= 360f;

                        scanInfos.Add(scanInfo);
                    }
                }

                lastSeenUnits = ship.Scan(scanInfos);

                Unit refUnit = null;

                foreach (Unit unit in lastSeenUnits)
                    if ((unit.Mobility == Mobility.Still || unit.Mobility == Mobility.Steady) 
                        && unit.Kind != UnitKind.Explosion
                        && refUnit == null || (refUnit != null && refUnit.Position.Length > unit.Position.Length))
                            refUnit = unit;

                if (refUnit != null)
                    referencingScanUnit = refUnit;
                else
                    return;

                if (lastSeenUnits.Count > 0)
                {
                    Map tempMap = new Map(gameScreen, ship, lastSeenUnits);

                    if (gameScreen.GlobalMap == null)
                        gameScreen.GlobalMap = tempMap;
                    else if (shipMap != null)
                    {
                        if (!shipMap.Merge(tempMap))
                        {
                            if (!gameScreen.GlobalMap.Merge(tempMap))
                                shipMap = tempMap;
                            else
                                shipMap = null;
                        }
                    }
                    else
                    {
                        if (!gameScreen.GlobalMap.Merge(tempMap))
                            shipMap = tempMap;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Scan: " + e.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Move()
        {
            try
            {
                Vector nextMove = null;

                MapUnit shipUnit = null;

                if (gameScreen.GlobalMap != null && !gameScreen.GlobalMap.TryGetUnit(ship.Name, out shipUnit))
                    if (shipMap != null)
                        shipMap.TryGetUnit(ship.Name, out shipUnit);

                Vector diff = null;

                if (referencingScanUnit != null)
                    diff = desiredMovePosition + referencingScanUnit.Movement;
                else
                    diff = desiredMovePosition;

                if (shipUnit != null && diff != null)
                {
                    nextMove = diff - shipUnit.Position;
                    nextMove = nextMove + shipUnit.Movement;

                    if (nextMove.Length < 0.125f)
                        nextMove.Length /= ship.EngineSpeed / 4f;
                    else if (nextMove.Length < 100f)
                        nextMove.Length /= ship.EngineSpeed;
                }

                if (nextMove != null && nextMove.Length > ship.EngineAcceleration.Limit * 0.99f)
                    nextMove.Length = ship.EngineAcceleration.Limit * 0.99f;

                /*foreach (Unit mUnit in mobileUnits)
                foreach (Unit sUnit in gravitalUnits)
                {
                    Vector diff = mUnit.Position - sUnit.Position;

                    if (diff < 100)
                        diff.Length = 100;

                    diff.Length = sUnit.Gravity * -100f / diff.Length;

                    mUnit.Movement += diff;
                }
                 */

                if (ship != null && ship.IsAlive && nextMove != null /*&& shipUnit.Movement.Length != ship.EngineSpeed*/)
                {
                    ship.Move(nextMove);
                }
                else
                {
                    Console.WriteLine("No move");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Move Error: " + e.Message);
            }
        }

        private void Shoot()
        {
            try
            {
                if (ship != null && ship.IsAlive)
                {
                    List<ShootCommand> tempShootCommands;

                    lock (syncShootCommands)
                    {
                        tempShootCommands = new List<ShootCommand>(shootCommands);
                        shootCommands = new List<ShootCommand>();
                    }

                    foreach (ShootCommand shootcommand in tempShootCommands)
                    {
                        Console.WriteLine(shootcommand.Time);

                        ship.Shoot(shootcommand.Direction,
                                   shootcommand.Direction.Angle,
                                   shootcommand.Time,
                                   shootcommand.Load,
                                   shootcommand.DamageHull,
                                   shootcommand.DamageShield,
                                   shootcommand.DamageEnergy,
                                   shootcommand.SubDirections);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Shoot: " + ex.Message);

                lock (syncShootCommands)
                    shootCommands = new List<ShootCommand>();
            }
        }

        private void Repair()
        {
            try
            {
                if (RepairSelf)
                {
                    if (ship.Energy / ship.EnergyMax > 0.5f && ship.Hull != ship.HullMax)
                        ship.RepairHull(ship.HullRepair.Limit);
                }
            }
            catch { }
        }

        private void LoadShields()
        {
            try
            {
                if (LoadShield)
                {
                    if (ship.ShieldMax > 0f && ship.Shield != ship.ShieldMax && ship.Energy / ship.EnergyMax > 0.5f)
                        ship.LoadShields(ship.ShieldLoad.Limit);
                }
            }
            catch { }
        }

        public void Continue()
        {
            //if (ship != null && !ship.IsAlive)
            try
            {
                ship.Continue();
            }
            catch
            {
                return;
            }

            lock (syncShootCommands)
                shootCommands = new List<ShootCommand>();

            if (lastSeenUnits != null)
                lastSeenUnits.Clear();
        }

        public void RemoveShip()
        {
            if (ship != null)
            {
                IsRunning = false;

                while (shipThread.IsAlive)
                    Thread.Sleep(1);

                ship.Close();
            }
        }

        public void MoveShip(Vector desiredMovePosition)
        {
            this.desiredMovePosition = desiredMovePosition;

            MapUnit shipUnit = null;

            if (gameScreen.GlobalMap != null && !gameScreen.GlobalMap.TryGetUnit(ship.Name, out shipUnit))
                if (shipMap != null)
                    shipMap.TryGetUnit(ship.Name, out shipUnit);

            List<MapUnit> mapUnits = MapUnits;

            foreach (MapUnit mapUnit in mapUnits)
                if (mapUnit.Mobility == MapUnitMobility.Still && mapUnit.Kind != MapUnitKind.MissionTarget && (mapUnit.Position - desiredMovePosition).Length <= (mapUnit.Radius + ship.Radius + 10f))
                {
                    Vector distance = shipUnit.Position - mapUnit.Position;

                    distance.Length = 3f + mapUnit.Radius + shipUnit.Radius;

                    this.desiredMovePosition = mapUnit.Position + distance;

                    break;
                }
        }

        public void Shoot(Vector desiredShootPosition)
        {
            MapUnit shipUnit = null;

            if (gameScreen.GlobalMap != null && !gameScreen.GlobalMap.TryGetUnit(ship.Name, out shipUnit))
                if (shipMap != null)
                    shipMap.TryGetUnit(ship.Name, out shipUnit);

            if (shipUnit != null)
            {
                // Where the shot has to be
                Vector desiredShotPosition = desiredShootPosition - shipUnit.Position;

                int timeInt = (1 + (int)(desiredShotPosition.Length / ship.WeaponShot.Speed.Limit * 0.99f));

                desiredShotPosition.Length = (desiredShotPosition.Length / timeInt);

                if (timeInt > ship.WeaponShot.Time.Limit)
                    timeInt = (int)(ship.WeaponShot.Time.Limit * 0.99f);

                desiredShotPosition = desiredShotPosition + shipUnit.Movement;

                try
                {
                    lock (syncShootCommands)
                    {
                        //desiredShotPosition.Angle -= 4f;

                        //for (int i = 0; i < 3; i++)
                        {
                            if (desiredShotPosition.Length > ship.WeaponShot.Speed.Limit * 0.99f)
                                desiredShotPosition.Length = ship.WeaponShot.Speed.Limit * 0.99f;

                            if (ship.WeaponProductionStatus >= 1.0f && ship.IsAlive)
                                shootCommands.Add(new ShootCommand(this, new Vector(desiredShotPosition),
                                                                   timeInt,
                                                                   ship.WeaponShot.Load.Limit * 0.85f,
                                                                   ship.WeaponShot.DamageHull.Limit * 0.95f,
                                                                   ship.WeaponShot.DamageShield.Limit * 0.75f,
                                                                   ship.WeaponShot.DamageEnergy.Limit * 0.75f,
                                                                   null));
                            //else
                            //    break;

                            //desiredShotPosition.Angle += 4f;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Shooting: " + ex.Message);
                }
            }
        }
        
    }
}
