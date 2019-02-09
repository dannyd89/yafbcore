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
        #region Fields and Properties
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
        private Dictionary<string, List<Map>> universeSortedMaps;

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
        private ManualResetEventSlim waitMergeEvent = new ManualResetEventSlim(false);

        /// <summary>
        /// Determines if this manager is disposed
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// Determines if this manager is disposed
        /// </summary>
        public bool IsDisposed => isDisposed;
        #endregion

        /// <summary>
        /// Creates a map manager
        /// </summary>
        /// <param name="universeGroup"></param>
        internal MapManager(UniverseSession universeSession)
        {
            Session = universeSession;
            UniverseGroup = universeSession.UniverseGroup;

            mainMaps = new Dictionary<string, Map>();
            universeSortedMaps = new Dictionary<string, List<Map>>();

            foreach (Universe universe in UniverseGroup.Universes)
            {
                mainMaps.Add(universe.Name, null);
                universeSortedMaps.Add(universe.Name, new List<Map>());
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
        /// Returns the searched player ship unit and the map containing it
        /// </summary>
        /// <param name="universeName">The universe to search the unit in</param>
        /// <param name="unitName">Unit name</param>
        /// <param name="map">The map the unit is in</param>
        /// <param name="playerShipMapUnit">The searched unit</param>
        /// <returns></returns>
        public bool TryGetPlayerUnit(string universeName, string unitName, out Map map, out PlayerShipMapUnit playerShipMapUnit)
        {
            map = null;
            playerShipMapUnit = null;

            Map tempMainMap;
            if (TryGetMap(universeName, out tempMainMap) && tempMainMap != null)
            {
                tempMainMap.BeginLock();

                if (!tempMainMap.TryGetPlayerShip(unitName, out playerShipMapUnit))
                {
                    lock (syncSortedMapsObj)
                    {
                        List<Map> universeMaps = universeSortedMaps[universeName];

                        if (universeMaps.Count > 1)
                            for (int i = 1; i < universeMaps.Count; i++)
                            {
                                Map tempMap = universeMaps[i];

                                try
                                {
                                    tempMap.BeginLock();

                                    if (tempMap.TryGetPlayerShip(unitName, out playerShipMapUnit))
                                    {
                                        map = tempMap;
                                        break;
                                    }
                                }
                                finally
                                {
                                    tempMap.EndLock();
                                }
                            }
                    }
                }
                else
                    map = tempMainMap;

                tempMainMap.EndLock();
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
                    universeSortedMaps = null;

                    waitMergeEvent.Dispose();
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

                universeSortedMaps[map.Universe.Name].Add(map);
            }
        }

        /// <summary>
        /// Enables to wait for the manager to signal if the maps are up2date
        /// </summary>
        internal void WaitMerge()
        {
            if (isDisposed)
                throw new ObjectDisposedException("MapManager is already disposed");

            waitMergeEvent.Wait();
        }

        /// <summary>
        /// Worker function which runs in a seperate thread
        /// </summary>
        private void worker()
        {
            using (UniverseGroupFlowControl flowControl = Session.UniverseGroup.GetNewFlowControl())
                while (!isDisposed)
                    try
                    {
                        flowControl.PreWait();

                        // Age the map because a tick has passed
                        //lock (syncSortedMapsObj)
                        //    foreach (KeyValuePair<string, List<Map>> kvp in universeSortedMaps)
                        //        foreach (Map map in kvp.Value)
                        //        {
                        //            map.BeginLock();
                        //            map.EndLock();
                        //        }

                        // Used to signal that the manager wants to merge
                        waitMergeEvent.Reset();

                        // Wait for all ships to finish
                        var ships = Session.ControllablesManager.Ships;
                        for (int i = 0; i < ships.Length; i++)
                            ships[i].ScanWaiter.Wait();

                        lock (syncSortedMapsObj)
                            foreach (KeyValuePair<string, List<Map>> kvp in universeSortedMaps)
                            {
                                List<Map> list = kvp.Value;
                                
                                if (list.Count > 0)
                                {
                                    foreach (Map map in list)
                                    {
                                        map.BeginLock();
                                        map.Age();
                                    }

                                    for (int i = 0; i < list.Count; i++)
                                    {
                                        // Determines if a successful merge has happend
                                        bool merged;

                                        do
                                        {
                                            merged = false;

                                            for (int n = list.Count - 1; n > i; n--)
                                            {
                                                Debug.Assert(list[i].Id != list[n].Id);

                                                if (list[i].Merge(list[n]))
                                                {
                                                    list[n].Dispose();
                                                    list.RemoveAt(n); // TODO: Think about a better way to do this since RemoveAt can be taxing

                                                    merged = true;
                                                }
                                            }
                                        } while (merged);
                                    }

                                    list.Sort();

                                    lock (syncMainMapObj)
                                        mainMaps[kvp.Key] = list[0];

                                    foreach (Map map in list)
                                    {
                                        if (map.IsUpdated)
                                            map.RaiseUpdated();

                                        map.EndLock();
                                    }
                                }
                            }

                        // Manager is done
                        waitMergeEvent.Set();

                        flowControl.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (isDisposed)
                            return; // Manager was disposed so we can stop this worker

                        Debug.WriteLine(ex);
                    }
        }
    }
}
