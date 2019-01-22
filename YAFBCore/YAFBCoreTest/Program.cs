using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using YAFBCore.Mapping;
using YAFBCore.Networking;

namespace YAFBCoreTest
{
    class Program
    {
        private static bool isRunning = true;

        private static Map globalMap;

        static unsafe void Main(string[] args)
        {
            //MapSectionRasterTilePackZero tileZero = new MapSectionRasterTilePackZero();

            //byte* addr = (byte*)&tileZero;
            //Console.WriteLine("Size:      {0}", sizeof(MapSectionRasterTilePackZero));
            //Console.WriteLine("Status Offset: {0}", &tileZero.Status - addr);
            //Console.WriteLine("ParentX Offset: {0}", (byte*)&tileZero.ParentX - addr);
            //Console.WriteLine("ParentY Offset: {0}", (byte*)&tileZero.ParentY - addr);
            //Console.WriteLine("Sum Offset: {0}", (byte*)&tileZero.Sum - addr);
            //Console.WriteLine("Gone Offset: {0}", (byte*)&tileZero.Gone - addr);

            Console.WriteLine();

            MapSectionRasterTile tile = new MapSectionRasterTile();

            byte* addr = (byte*)&tile;
            Console.WriteLine("Size:      {0}", sizeof(MapSectionRasterTile));
            Console.WriteLine("Status Offset: {0}", &tile.Status - addr);
            Console.WriteLine("ParentX Offset: {0}", (byte*)&tile.ParentX - addr);
            Console.WriteLine("ParentY Offset: {0}", (byte*)&tile.ParentY - addr);
            Console.WriteLine("Sum Offset: {0}", (byte*)&tile.Sum - addr);
            Console.WriteLine("Gone Offset: {0}", (byte*)&tile.Gone - addr);

            //var tileZeroArray = new MapSectionRasterTilePackZero[ushort.MaxValue * 2];
            var tileArray = new MapSectionRasterTile[ushort.MaxValue * 2];

            //for (int i = 0; i < 5; i++)
            //{
            //    Stopwatch sw = Stopwatch.StartNew();
            //    int count = 0;
            //    for (int a = 0; a < 1_000_000; a++)
            //        for (int x = 0; x < tileZeroArray.Length; x++)
            //        {
            //            count += tileZeroArray[x].Sum++;
            //        }

            //    Console.WriteLine("Pack=0 Time: " + sw.Elapsed);

            //    sw.Restart();

            //    count = 0;
            //    for (int a = 0; a < 1_000_000; a++)
            //        for (int x = 0; x < tileArray.Length; x++)
            //        {
            //            count += tileArray[x].Sum++;
            //        }

            //    Console.WriteLine("Pack=1 Time: " + sw.Elapsed);
            //}

            //Console.WriteLine("128 elements Pack=0: " + tileZeroArray.Length * sizeof(MapSectionRasterTilePackZero));

            //Connection connection = ConnectionManager.Connect("ddraghici@gmx.de", "flattiverse=1337");

            //Flattiverse.UniverseGroup universeGroup = connection.UniverseGroups["Time Master"];
            //UniverseSession session = connection.Join(universeGroup, "dannyd", universeGroup.Teams["None"]);

            //System.Threading.ThreadPool.QueueUserWorkItem(worker, session);

            //connection.MessageManager.ReadMessages();

            Console.ReadKey();
        }

        static void worker(object state)
        {
            //UniverseSession session = state as UniverseSession;
            //UniverseGroupFlowControlWrapper flowControl = null;

            //float scanDirection = 0f;

            //flowControl = session.CreateFlowControl();

            //var ship = session.ControllablesManager.CreateShip("D1RP", "D1RP");
            //ship.TryContinue();

            //Console.CursorVisible = false;

            //while (isRunning)
            //{
            //    try
            //    {
            //        Console.SetCursorPosition(0, 0);

            //        //flowControl.PreWait();

            //        flowControl.Wait();

            //        flowControl.Commit();
            //    }
            //    catch (NullReferenceException)
            //    {
            //        Console.WriteLine();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //        Console.WriteLine(ex.StackTrace);
            //        if (ship != null && ship.IsAlive)
            //            continue;

            //        Console.Clear();
            //    }
            //}
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
