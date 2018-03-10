using System;
using System.Collections.Generic;
using System.Text;
using YAFBCore.Mapping;
using YAFBCore.Networking;

namespace YAFBCoreTest
{
    class Program
    {
        private static bool isRunning = true;

        private static Map globalMap;

        static void Main(string[] args)
        {
            Connection connection = ConnectionManager.Connect("kriegsviech@web.de", "flattiverse1337");

            Flattiverse.UniverseGroup universeGroup = connection.UniverseGroups["Time Master"];
            UniverseSession session = connection.Join(universeGroup, "dannyTest", universeGroup.Teams["None"]);

            System.Threading.ThreadPool.QueueUserWorkItem(worker, session);

            Console.ReadKey();
        }

        static void worker(object state)
        {
            UniverseSession session = state as UniverseSession;
            UniverseGroupFlowControlWrapper flowControl = null;

            float scanDirection = 0f;
            Flattiverse.Ship ship = null;

            flowControl = session.CreateFlowControl();

            ship = session.CreateShip("D3RPTest", "D3RPTest");
            ship.Continue();

            Console.CursorVisible = false;

            while (isRunning)
            {
                try
                {
                    Console.SetCursorPosition(0, 0);

                    flowControl.PreWait();

                    #region Scan
                    List<Flattiverse.ScanInfo> scanInfos = new List<Flattiverse.ScanInfo>();

                    for (int i = 0; i < ship.ScannerCount; i++)
                    {
                        Flattiverse.ScanInfo scanInfo = new Flattiverse.ScanInfo(scanDirection, scanDirection + ship.ScannerDegreePerScan, ship.ScannerArea.Limit * 0.99f);

                        scanDirection += ship.ScannerDegreePerScan;

                        if (scanDirection >= 360f)
                            scanDirection -= 360f;

                        scanInfos.Add(scanInfo);
                    }

                    List<Flattiverse.Unit> units = ship.Scan(scanInfos);

                    Map map = Map.Create(ship, units);

                    if (map != null)
                    {
                        if (globalMap == null)
                            globalMap = map;
                        else
                        {
                            globalMap.BeginLock();
                            map.BeginLock();

                            globalMap.Merge(map);

                            globalMap.Age();

                            globalMap.DebugPrint();

                            globalMap.EndLock();
                            map.EndLock();

                            map.Dispose();
                        }
                    }
                    #endregion

                    flowControl.Wait();

                    Flattiverse.Unit refUnit = null;

                    foreach (Flattiverse.Unit unit in units)
                        if ((unit.Mobility == global::Flattiverse.Mobility.Still)
                            && unit.Kind != global::Flattiverse.UnitKind.Explosion
                            && (refUnit == null || (refUnit != null && refUnit.Position.Length > unit.Position.Length)))
                            refUnit = unit;

                    ship.Move(refUnit.Movement);

                    flowControl.Commit();

                }
                catch (NullReferenceException)
                {
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (ship != null && ship.IsAlive)
                        continue;

                    if (ship != null && !ship.IsAlive)
                        ship.Continue();

                    Console.Clear();
                    //Console.WriteLine(ex.StackTrace);
                }
            }
            //finally
            //{
            //    if (flowControl != null)
            //        session.RemoveFlowControl(flowControl);

            //    Console.WriteLine("Crashed...");

            //    session.Dispose();
            //}
        }
    }
}
