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
            Connection connection = ConnectionManager.Connect("ddraghici@gmx.de", "flattiverse=1337");

            Flattiverse.UniverseGroup universeGroup = connection.UniverseGroups["Time Master"];
            UniverseSession session = connection.Join(universeGroup, "dannyd", universeGroup.Teams["None"]);

            System.Threading.ThreadPool.QueueUserWorkItem(worker, session);

            connection.MessageManager.ReadMessages();

            Console.ReadKey();
        }

        static void worker(object state)
        {
            UniverseSession session = state as UniverseSession;
            UniverseGroupFlowControlWrapper flowControl = null;

            float scanDirection = 0f;

            flowControl = session.CreateFlowControl();

            var ship = session.ControllablesManager.CreateShip("D1RP", "D1RP");
            ship.TryContinue();

            Console.CursorVisible = false;

            while (isRunning)
            {
                try
                {
                    Console.SetCursorPosition(0, 0);

                    //flowControl.PreWait();

                    flowControl.Wait();

                    flowControl.Commit();
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    if (ship != null && ship.IsAlive)
                        continue;

                    Console.Clear();
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
