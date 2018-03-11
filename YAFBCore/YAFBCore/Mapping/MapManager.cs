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
        internal MapManager(UniverseSession universeSession)
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
        /// Disposes this map manager
        /// </summary>
        public void Dispose()
        {
            lock (syncObj)
            {
                if (isDisposed)
                    throw new ObjectDisposedException("MapManager is already disposed");

                isDisposed = true;

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
            lock (syncObj)
            {
                if (isDisposed)
                    throw new ObjectDisposedException("MapManager is already disposed");

                sortedMaps[map.Universe.Name].Add(map);
            }
        }

        /// <summary>
        /// Enables to wait for the manager to signal if the maps are up2date
        /// </summary>
        internal void Wait()
        {
            if (isDisposed)
                throw new ObjectDisposedException("MapManager is already disposed");

            waitEvent.Wait();
        }

        /// <summary>
        /// Worker function which runs in a seperate thread
        /// </summary>
        private void worker()
        {
            while (!isDisposed)
                try
                {
                    flowControl.FlowControl.PreWait();

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

                    flowControl.FlowControl.Commit();
                }
                catch
                {
                    if (isDisposed)
                        break; // Manager was disposed so we can stop this worker
                }
        }
    }
}
