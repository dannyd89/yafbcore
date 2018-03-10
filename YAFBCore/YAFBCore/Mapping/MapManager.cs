using Flattiverse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using YAFBCore.Networking;

namespace YAFBCore.Mapping
{
    public class MapManager : IDisposable
    {
        /// <summary>
        /// List of all map managers
        /// </summary>
        private static Dictionary<string, MapManager> Managers = new Dictionary<string, MapManager>();

        /// <summary>
        /// Used to synchronize the Managers field
        /// </summary>
        private static object staticSyncObj = new object();

        /// <summary>
        /// The session this manager was created in
        /// </summary>
        public readonly UniverseSession UniverseSession;

        /// <summary>
        /// Active universe group for this manager
        /// </summary>
        internal readonly UniverseGroup UniverseGroup;

        /// <summary>
        /// The flow control for the active universe group
        /// </summary>
        private UniverseGroupFlowControlWrapper flowControl;

        /// <summary>
        /// Main maps for each universe in a group
        /// </summary>
        private Dictionary<string, Map> mainMaps;

        /// <summary>
        /// A dictionary to sort maps according to the available universes in a group
        /// </summary>
        private Dictionary<string, List<Map>> sortedMaps;

        /// <summary>
        /// Worker thread to merge maps etc.
        /// </summary>
        private Thread workerThread;

        /// <summary>
        /// Used to synchronize this manager
        /// </summary>
        private object syncObj = new object();

        /// <summary>
        /// Wait event so anything can wait for the map manager to do its work
        /// </summary>
        private ManualResetEventSlim waitEvent = new ManualResetEventSlim(false);

        /// <summary>
        /// Determines if this manager is disposed
        /// </summary>
        private volatile bool isDisposed;
        public bool IsDisposed => isDisposed;

        /// <summary>
        /// Creates a map manager
        /// </summary>
        /// <param name="universeGroup"></param>
        private MapManager(UniverseSession universeSession)
        {
            UniverseSession = universeSession;
            UniverseGroup = universeSession.UniverseGroup;

            flowControl = universeSession.CreateFlowControl();

            mainMaps = new Dictionary<string, Map>();
            sortedMaps = new Dictionary<string, List<Map>>();
            foreach (Universe universe in UniverseGroup.Universes)
            {
                mainMaps.Add(universe.Name, null);
                sortedMaps.Add(universe.Name, new List<Map>());
            }
            
            workerThread = new Thread(new ThreadStart(worker));
            workerThread.Start();
        }

        /// <summary>
        /// Either creates a new map manager if there isnt one for this universe group, else returns an existing one
        /// </summary>
        /// <param name="universeSession">Universe group where the session is running on</param>
        /// <returns>Returns a map manager</returns>
        internal static MapManager Create(UniverseSession universeSession)
        {
            lock (staticSyncObj)
            {
                MapManager mapManager;
                if (Managers.TryGetValue(universeSession.UniverseGroup.Name, out mapManager))
                    return mapManager;

                mapManager = new MapManager(universeSession);
                Managers.Add(universeSession.Name, mapManager);

                return mapManager;
            }
        }

        /// <summary>
        /// Returns the map manager responsible for the passed universe group
        /// </summary>
        /// <param name="universeGroupName"></param>
        /// <returns>True if manager was found</returns>
        public static bool TryGetManager(string universeGroupName, out MapManager mapManager)
        {
            lock (staticSyncObj)
                return Managers.TryGetValue(universeGroupName, out mapManager);
        }

        /// <summary>
        /// Disposes this map manager
        /// </summary>
        public void Dispose()
        {
            if (isDisposed)
                throw new InvalidOperationException("MapManager is already disposed");

            isDisposed = true;

            lock (staticSyncObj)
                Managers.Remove(UniverseGroup.Name);

            lock (syncObj)
            {
                UniverseSession.RemoveFlowControl(flowControl);
                waitEvent.Dispose();
            }
        }

        /// <summary>
        /// Adds a new map to the list which will be managed by this class (merging/aging)
        /// </summary>
        /// <param name="map"></param>
        internal void Add(Map map)
        {
            if (isDisposed)
                throw new InvalidOperationException("MapManager is already disposed");

            lock (syncObj)
                sortedMaps[map.Universe.Name].Add(map);
        }

        /// <summary>
        /// Enables to wait for the manager to signal if the maps are up2date
        /// </summary>
        internal void Wait()
        {
            if (isDisposed)
                throw new InvalidOperationException("MapManager is already disposed");

            waitEvent.Wait();
        }

        /// <summary>
        /// Worker function which runs in a seperate thread
        /// </summary>
        private void worker()
        {
            while (true)
                try
                {
                    flowControl.PreWait();

                    lock (syncObj)
                        foreach (KeyValuePair<string, List<Map>> kvp in sortedMaps)
                            for (int i = 0; i < kvp.Value.Count; i++)
                            {
                                kvp.Value[i].BeginLock();
                                kvp.Value[i].Age();
                                kvp.Value[i].EndLock();
                            }

                    // Used to signal that the manager wants to merge
                    waitEvent.Reset();

                    // We dont need to call Wait() here because we need to wait for all the controllables anyway
                    //flowControl.Wait();

                    // TODO: We need to wait for all controllables to be done with scanning

                    lock (syncObj)
                        foreach (KeyValuePair<string, List<Map>> kvp in sortedMaps)
                        {
                            List<Map> list = kvp.Value;

                            if (list.Count > 0)
                            {
                                for (int i = 0; i < list.Count; i++)
                                {
                                    list[i].BeginLock();

                                    // Determines if a successful merge has happend
                                    bool merged = false;

                                    do
                                    {
                                        for (int n = list.Count - 1; n > i; n--)
                                        {
                                            Debug.Assert(list[i].Id != list[n].Id);

                                            list[n].BeginLock();

                                            if (list[i].Merge(list[n]))
                                            {
                                                list[n].Dispose();
                                                list.RemoveAt(n);

                                                merged = true;
                                                Debug.WriteLine("Merge successful");

                                                continue; // We need to continue since we disposed the map already and EndLock cant be called
                                            }
                                            else
                                            {
                                                Debug.WriteLine("Merge not successful");

                                                list[i].DebugPrint();
                                                list[n].DebugPrint();
                                            }

                                            list[n].EndLock();
                                        }
                                    } while (merged);

                                    list[i].EndLock();
                                }

                                list.Sort();
                            }
                        }

                    // Manager is done
                    waitEvent.Set();

                    flowControl.Commit();
                }
                catch
                {
                    if (isDisposed)
                        break; // Manager was disposed so we can stop this worker
                }
        }
    }
}
