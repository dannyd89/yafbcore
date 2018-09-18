using Flattiverse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using YAFBCore.Mapping.Units;
using YAFBCore.Networking;

namespace YAFBCore.Mapping
{
    public class MapManager : IDisposable
    {
        /// <summary>
        /// The session this manager was created in
        /// </summary>
        public readonly UniverseSession Session;

        /// <summary>
        /// Active universe group for this manager
        /// </summary>
        internal readonly UniverseGroup UniverseGroup;

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
        /// Used to synchronize sortedMaps
        /// </summary>
        private object syncSortedMapsObj = new object();

        /// <summary>
        /// Used to synchronize mainMaps
        /// </summary>
        private object syncMainMapObj = new object();

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
            Session = universeSession;
            UniverseGroup = universeSession.UniverseGroup;

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
        /// Returns the main map for the given universe name
        /// </summary>
        /// <param name="universeName">Universe name</param>
        /// <returns>Current main map, may be null if there is no main map yet</returns>
        public Map this[string universeName]
        {
            get
            {
                lock (syncMainMapObj)
                    return mainMaps[universeName];
            }
        }

        /// <summary>
        /// Tries to return the main map for the given universe name
        /// </summary>
        /// <param name="universeName">Universe name</param>
        /// <param name="map">Current main map</param>
        /// <returns></returns>
        public bool TryGetMap(string universeName, out Map map)
        {
            lock (syncMainMapObj)
                return mainMaps.TryGetValue(universeName, out map);
        }

        /// <summary>
        /// Tries to return the units in the given viewport
        /// </summary>
        /// <param name="universeName">Name of the universe</param>
        /// <param name="viewport"></param>
        /// <param name="units"></param>
        /// <returns></returns>
        public bool TryGetUnits(string universeName, Utils.Mathematics.RectangleF viewport, out List<MapUnit> units)
        {
            units = null;

            Map tempMap;
            if (TryGetMap(universeName, out tempMap) && tempMap != null)
            {
                tempMap.BeginLock();
                units = tempMap.GetUnits(viewport);
                tempMap.EndLock();
            }

            return units != null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="universeName"></param>
        /// <param name="unitName"></param>
        /// <param name="playerShipMapUnit"></param>
        /// <returns></returns>
        public bool TryGetPlayerUnit(string universeName, string unitName, out PlayerShipMapUnit playerShipMapUnit)
        {
            playerShipMapUnit = null;

            Map tempMap;
            if (TryGetMap(universeName, out tempMap) && tempMap != null)
            {
                tempMap.BeginLock();

                if (!tempMap.TryGetPlayerShip(unitName, out playerShipMapUnit))
                    Debug.WriteLine("Didn't find the requested unit " + unitName + " in the given universe " + universeName);

                tempMap.EndLock();
            }

            return playerShipMapUnit != null;
        }

        /// <summary>
        /// Disposes this map manager
        /// </summary>
        public void Dispose()
        {
            lock (syncSortedMapsObj)
                lock (syncMainMapObj)
                {
                    if (isDisposed)
                        throw new ObjectDisposedException("MapManager is already disposed");

                    isDisposed = true;

                    mainMaps = null;
                    sortedMaps = null;

                    waitEvent.Dispose();
                }
        }

        /// <summary>
        /// Adds a new map to the list which will be managed by this class (merging/aging)
        /// </summary>
        /// <param name="map"></param>
        internal void Add(Map map)
        {
            lock (syncSortedMapsObj)
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
            using (UniverseGroupFlowControl flowControl = UniverseGroup.GetNewFlowControl())
                while (!isDisposed)
                    try
                    {
                        flowControl.PreWait();

                        // Age the map because a tick has passed
                        lock (syncSortedMapsObj)
                            foreach (KeyValuePair<string, List<Map>> kvp in sortedMaps)
                                for (int i = 0; i < kvp.Value.Count; i++)
                                {
                                    kvp.Value[i].BeginLock();
                                    kvp.Value[i].Age();
                                    kvp.Value[i].EndLock();
                                }

                        // Used to signal that the manager wants to merge
                        waitEvent.Reset();

                        // Wait for all ships to finish
                        var ships = Session.ControllablesManager.Ships;
                        for (int i = 0; i < ships.Length; i++)
                            ships[i].ScanWaiter.Wait();

                        lock (syncSortedMapsObj)
                            foreach (KeyValuePair<string, List<Map>> kvp in sortedMaps)
                            {
                                List<Map> list = kvp.Value;

                                if (list.Count > 0)
                                {
                                    for (int i = 0; i < list.Count; i++)
                                    {
                                        list[i].BeginLock();

                                        // Determines if a successful merge has happend
                                        bool merged;

                                        do
                                        {
                                            merged = false;

                                            for (int n = list.Count - 1; n > i; n--)
                                            {
                                                Debug.Assert(list[i].Id != list[n].Id);

                                                list[n].BeginLock();

                                                if (list[i].Merge(list[n]))
                                                {
                                                    list[n].Dispose();
                                                    list.RemoveAt(n);

                                                    merged = true;

                                                    // We need to continue since we disposed the map already and EndLock cant be called
                                                    continue;
                                                }
                                                //else
                                                //{
                                                //    Debug.WriteLine("Merge not successful");

                                                //    list[i].DebugPrint();
                                                //    list[n].DebugPrint();
                                                //}

                                                list[n].EndLock();
                                            }
                                        } while (merged);

                                        list[i].EndLock();
                                    }

                                    list.Sort();

                                    lock (syncMainMapObj)
                                        mainMaps[kvp.Key] = list[0];
                                }
                            }

                        // Manager is done
                        waitEvent.Set();

                        flowControl.Commit();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);

                        if (isDisposed)
                            break; // Manager was disposed so we can stop this worker
                    }
        }
    }
}
