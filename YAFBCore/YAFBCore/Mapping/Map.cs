using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flattiverse;
using YAFBCore.Mapping.Units;
using YAFBCore.Extensions;
using YAFBCore.Utils;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using YAFBCore.Utils.Mathematics;
using YAFBCore.Pathfinding.Pathfinders;

namespace YAFBCore.Mapping
{
    public delegate void MapUpdatedEventHandler(Map map);
    
    public class Map : IDisposable, IComparable<Map>
    {
        #region Fields and Properties
        /// <summary>
        /// Id counter
        /// </summary>
        private static long counter = 0;

        /// <summary>
        /// Id of this object
        /// </summary>
        public readonly long Id;

        /// <summary>
        /// Defines the width and height of a section
        /// </summary>
        internal const int SectionSize = 1024;

        /// <summary>
        /// The universe this map belongs to
        /// </summary>
        public readonly Universe Universe;

        /// <summary>
        /// Holding all known still units in this map for faster merging
        /// </summary>
        private Dictionary<string, MapUnit> stillUnits = new Dictionary<string, MapUnit>();

        /// <summary>
        /// List to find own player units faster
        /// </summary>
        private Dictionary<string, PlayerShipMapUnit> ownPlayerUnits = new Dictionary<string, PlayerShipMapUnit>();

        /// <summary>
        /// Map is divided into sections to optimize path finding
        /// </summary>
        private MapSection[] mapSections;

        /// <summary>
        /// Map section count in one dimension
        /// Default: 3
        /// Gets increased by 2^sectionExtensionCounter every time needed
        /// </summary>
        private int sectionCount = 3;

        /// <summary>
        /// Defines by how many sections the map gets enlarged if needed
        /// </summary>
        private int sectionExtensionCounter = 1;

        /// <summary>
        /// Transformator to calculate in which section an unit has to be placed
        /// Also works the other way around
        /// </summary>
        private Transformator transformator;

        /// <summary>
        /// WaitHandle to lock the map to secure a consistent state for each call
        /// </summary>
        private AutoResetEvent lockMapEvent = new AutoResetEvent(true);

        /// <summary>
        /// Id of the thread currently locking the map
        /// </summary>
        private int lockingThreadId;

        /// <summary>
        /// States whether the map is locked or not
        /// </summary>
        private bool isLocked = false;

        /// <summary>
        /// 
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// True when Dispose() was called
        /// </summary>
        public bool IsDisposed => isDisposed;

        /// <summary>
        /// 
        /// </summary>
        private bool isUpdated = true;

        /// <summary>
        /// 
        /// </summary>
        internal bool IsUpdated
        {
            get => isUpdated;
            set => isUpdated = value;
        }

        /// <summary>
        /// List of ships observing this map for updates
        /// </summary>
        private List<Controllables.Ship> observer = new List<Controllables.Ship>();
        #endregion

        #region Events
        private MapUpdatedEventHandler _updatedEventHandler;
        /// <summary>
        /// Called when the unit data of the map has changed
        /// </summary>
        public event MapUpdatedEventHandler Updated
        {
            add
            {
                _updatedEventHandler += value;

                if (value.Target is Controllables.Ship)
                    observer.Add((Controllables.Ship)value.Target);
            }
            remove
            {
                _updatedEventHandler -= value;

                if (value.Target is Controllables.Ship)
                    observer.Remove((Controllables.Ship)value.Target);
            }
        }
        #endregion

        #region Constructors or Create functions
        /// <summary>
        /// Private constructor to set Id
        /// </summary>
        private Map(Universe universe)
        {
            Id = counter++;
            Universe = universe;

            enlargeMap();
        }

        /// <summary>
        /// Creates a new map based upon a recent scan
        /// </summary>
        /// <param name="creator">Unit which scanned the passed units</param>
        /// <param name="units">Units which were scanned</param>
        public static Map Create(Controllables.Controllable creator, List<Unit> units)
        {
#if DEBUG
            try
            {
#endif
                Vector movementOffset = Vector.FromNull();
                foreach (Unit unit in units)
                    if (unit.Kind != UnitKind.Explosion
                        && unit.Mobility == Mobility.Still)
                    {
                        movementOffset = unit.Movement;
                        break;
                    }

                movementOffset = new Flattiverse.Vector(-movementOffset.X, -movementOffset.Y);

                Universe universe = creator.Base.Universe;
                string currentPlayerName = universe.Connector.Player.Name;

                Map map = new Map(universe);

                MapUnit mapUnit;
                foreach (Unit unit in units)
                {
                    if (unit is PlayerUnit playerUnit && playerUnit.Player.Name == currentPlayerName)
                        continue;

                    mapUnit = MapUnitFactory.GetMapUnit(map, unit, movementOffset);

                    map.mapSections[map.getMapSectionIndex(mapUnit.PositionInternal.X, mapUnit.PositionInternal.Y)].AddOrUpdate(mapUnit);

                    if (mapUnit.Mobility == Mobility.Still)
                        map.stillUnits.Add(mapUnit.Name, mapUnit);
                }

                mapUnit = MapUnitFactory.GetMapUnit(map, creator.Base, movementOffset);
                map.mapSections[map.getMapSectionIndex(mapUnit.PositionInternal)].AddOrUpdate(mapUnit);

                return map;
#if DEBUG
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);

                return null;
            }
#endif
        }
        #endregion

        #region Public functions
        /// <summary>
        /// <para>Locks map for current thread to read/edit.</para>
        /// Execute before calling any functions who want to read/edit the map, or if you want a consistent state of the map for a specific call
        /// </summary>
        public void BeginLock()
        {
            lockMapEvent.WaitOne();

            if (isDisposed)
                throw new ObjectDisposedException("Map is already disposed");

            lockingThreadId = Thread.CurrentThread.ManagedThreadId;
            isLocked = true;
        }

        /// <summary>
        /// Releases the lock for other threads to edit
        /// </summary>
        public void EndLock()
        {
            if (isDisposed)
                throw new ObjectDisposedException("Map is already disposed");

            lockingThreadId = -1;
            isLocked = false;

            lockMapEvent.Set();
        }

        /// <summary>
        /// Tries to merge the two maps with each other
        /// </summary>
        /// <param name="other">The other map to merge with</param>
        /// <returns>Returns true if merge was successful</returns>
        internal bool Merge(Map other)
        {
            if (isDisposed)
                throw new ObjectDisposedException("Map is already disposed");

#if DEBUG
            if (!isLocked || !other.isLocked)
                throw new InvalidOperationException("Please acquire a lock on both maps before trying to merge them");

            if (lockingThreadId != other.lockingThreadId || lockingThreadId != Thread.CurrentThread.ManagedThreadId)
                throw new InvalidOperationException("Another thread is currently locking this, please aquire your own lock");

            if (Universe.Name != other.Universe.Name)
                return false;
#endif

            Vector positionOffset = null;

            MapUnit mapUnit;
            foreach (KeyValuePair<string, MapUnit> unitKvp in other.stillUnits)
                if (stillUnits.TryGetValue(unitKvp.Key, out mapUnit))
                {
                    positionOffset = mapUnit.PositionInternal - unitKvp.Value.PositionInternal;
                    break;
                }

            // No reference point found between both maps
            if (positionOffset == null)
                return false;

            for (int otherIndex = 0; otherIndex < other.mapSections.Length; otherIndex++)
            {
                MapSection mapSection = other.mapSections[otherIndex];
                MapUnit[] tempStillUnits = mapSection.StillUnits;

                if (mapSection.StillCount > 0)
                {
                    addOrUpdateUnits(tempStillUnits, positionOffset);

                    isUpdated = true;
                }

                for (int i = 0; i < tempStillUnits.Length; i++)
                {
                    if (tempStillUnits[i] == null)
                        break;

                    if (!stillUnits.TryGetValue(tempStillUnits[i].Name, out mapUnit))
                        stillUnits.Add(tempStillUnits[i].Name, tempStillUnits[i]);
                }

                if (mapSection.AgingCount > 0)
                {
                    addOrUpdateUnits(mapSection.AgingUnits, positionOffset);

                    isUpdated = true;
                }
            }
            
            for (int i = other.observer.Count - 1; i >= 0; i--)
                if (!observer.Contains(other.observer[i]))
                {
                    Updated += other.observer[i].Map_MapUpdated;
                    other.Updated -= other.observer[i].Map_MapUpdated;
                }

            return true;
        }

        /// <summary>
        /// Ages the map for one tick.
        /// </summary>
        internal void Age()
        {
            if (isDisposed)
                throw new InvalidOperationException("Map is already disposed");

#if DEBUG
            if (!isLocked)
                throw new InvalidOperationException("Please acquire a lock on this map");

            if (lockingThreadId != Thread.CurrentThread.ManagedThreadId)
                throw new InvalidOperationException("Another thread is currently locking this, please aquire your own lock");
#endif

            for (int i = 0; i < mapSections.Length; i++)
            {
                MapSection mapSection = mapSections[i];
                int count = mapSection.AgingCount;

                if (count > 0)
                {
                    for (int x = count - 1; x >= 0; x--)
                    {
                        MapUnit mapUnit = mapSection.AgingUnits[x];

                        Debug.Assert(mapUnit != null);

                        if (!mapUnit.Age())
                        {
                            mapSection.AgingUnits[x] = null;

                            if (mapUnit is PlayerShipMapUnit)
                                for(int p = 0; p < mapSection.PlayerCount; p++)
                                {
                                    Debug.Assert(mapSection.PlayerUnits[p] != null);

                                    if (mapSection.PlayerUnits[p].Name == mapUnit.Name)
                                        mapSection.PlayerUnits[p] = null;
                                }
                        }
                    }

                    mapSection.Sort(MapSectionSortType.AgingUnits | MapSectionSortType.PlayerUnits);

                    isUpdated = true;
                }
            }
        }

        /// <summary>
        /// MapManager raises the Updated event with this function
        /// </summary>
        internal void RaiseUpdated()
        {
            if (isUpdated)
            {
                isUpdated = false;

                _updatedEventHandler?.Invoke(this);
            }
        }

        /// <summary>
        /// Gets the units which are visible in the passed viewport
        /// Maps needs to be locked before calling this
        /// </summary>
        /// <param name="viewport">Viewport which describes the area the game is currently looking at. It's highly advised to span the passed viewport a bit larger than the actual viewport.</param>
        /// <returns>A list of units which are contained in the viewport</returns>
        internal List<MapUnit> GetUnits(RectangleF viewport)
        {
            if (isDisposed)
                throw new InvalidOperationException("Map is already disposed");

#if DEBUG
            if (!isLocked)
                throw new InvalidOperationException("Please acquire a lock on this map");

            if (lockingThreadId != Thread.CurrentThread.ManagedThreadId)
                throw new InvalidOperationException("Another thread is currently locking this, please aquire your own lock");
#endif

            List<MapUnit> mapUnits = new List<MapUnit>(300);

            int startIndex = getMapSectionIndex(viewport.Left, viewport.Top);
            int maxIndex = getMapSectionIndex(viewport.Right, viewport.Bottom);

            int playerCount = 0;
            for (int i = startIndex; i <= maxIndex; i++)
            {
                MapSection mapSection = mapSections[i];

                if (mapSection.Bounds.Intersects(viewport))
                {
                    for (int x = 0; x < mapSection.StillCount; x++)
                        mapUnits.Add(mapSection.StillUnits[x]);

                    for (int x = 0; x < mapSection.AgingCount; x++)
                        if (!mapUnits.Contains(mapSection.AgingUnits[x]))
                        {
                            mapUnits.Add(mapSection.AgingUnits[x]);

                            if (mapSection.AgingUnits[x] is PlayerShipMapUnit)
                                playerCount++;
                        }

                    for (int x = 0; x < mapSection.PlayerCount; x++)
                        if (!mapUnits.Contains(mapSection.PlayerUnits[x]))
                        {
                            mapUnits.Add(mapSection.PlayerUnits[x]);
                            playerCount++;
                        }
                }
            }

            if (playerCount == 0)
                Debug.WriteLine("Player not found");

            //Debug.WriteLine("Startindex: " + startIndex + " maxIndex: " + maxIndex + " returning unit count: " + mapUnits.Count + " for map (#" + Id.ToString().PadLeft(4, '0') + ")");

            return mapUnits;
        }

        /// <summary>
        /// Searches the given player ship
        /// </summary>
        /// <param name="unitName">Name of the player ship</param>
        /// <param name="playerShipMapUnit"></param>
        /// <returns>true if unit was found</returns>
        internal bool TryGetPlayerShip(string unitName, out PlayerShipMapUnit playerShipMapUnit)
        {
            if (isDisposed)
                throw new InvalidOperationException("Map is already disposed");

#if DEBUG
            if (!isLocked)
                throw new InvalidOperationException("Please acquire a lock on this map");

            if (lockingThreadId != Thread.CurrentThread.ManagedThreadId)
                throw new InvalidOperationException("Another thread is currently locking this, please aquire your own lock");
#endif

            playerShipMapUnit = null;

            for (int i = 0; i < mapSections.Length; i++)
            {
                MapSection mapSection = mapSections[i];

                for (int x = 0; x < mapSection.PlayerCount; x++)
                    if (mapSection.PlayerUnits[x].Name == unitName)
                    {
                        // TODO: Hier müsste eine Überprüfung noch rein, weil es nicht immer nur PlayerShips sein müssen
                        playerShipMapUnit = (PlayerShipMapUnit)mapSection.PlayerUnits[x];
                        return true;
                    }
            }

            return false;
        }

        /// <summary>
        /// Returns a path finder with a specific tile size
        /// Does not generate a path finder
        /// If tile size is unknown an exception will happen
        /// </summary>
        /// <param name="tileSize">Requested tile size of the path finder</param>
        internal MapPathfinder GetPathFinder(int tileSize)
        {
            if (isDisposed)
                throw new InvalidOperationException("Map is already disposed");

            return new MapPathfinder(tileSize, this, mapSections, sectionCount);
        }

        /// <summary>
        /// Used for sorting maps.
        /// <para>Sort Type: DESC</para>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Map other)
        {
#if DEBUG
            if (isDisposed || other.isDisposed)
                throw new InvalidOperationException("Map is already disposed");

            if (other == null)
                return -1;
#endif 

            // Sort it desc
            return -unitCount().CompareTo(other.unitCount());
        }

        /// <summary>
        /// Disposes the map
        /// </summary>
        public void Dispose()
        {
#if DEBUG
            if (isDisposed)
                throw new InvalidOperationException("Map is already disposed");
#endif

            isDisposed = true;

            mapSections = null;
            stillUnits = null;

            lockMapEvent.Dispose();
        }
        #endregion

        #region Private functions
        /// <summary>
        /// Enlarges the mapSections array
        /// </summary>
        private void enlargeMap()
        {
            if (mapSections == null)
            {
                mapSections = new MapSection[sectionCount * sectionCount];

                float size = (sectionCount / 2f) * SectionSize;
                transformator = new Transformator(-size, size, 0, sectionCount);

                for (int i = 0; i < mapSections.Length; i++)
                    mapSections[i] = new MapSection(this, new RectangleF(transformator.Rev(i % sectionCount), transformator.Rev(i / sectionCount), SectionSize, SectionSize));
            }
            else
            {
                int tempCount = 3 + (int)Math.Pow(2, sectionExtensionCounter++);

                var temp = new MapSection[tempCount * tempCount];

                float size = (tempCount / 2f) * SectionSize;
                transformator = new Transformator(-size, size, 0, tempCount);

                for (int y = 0; y < tempCount; y++)
                    for (int x = 0; x < tempCount; x++)
                    {
                        if (y == 0 || y == tempCount - 1 || x == 0 || x == tempCount - 1)
                            temp[x + y * tempCount] = new MapSection(this, new RectangleF(transformator.Rev(x), transformator.Rev(y), SectionSize, SectionSize));
                        else
                            temp[x + y * tempCount] = mapSections[(x - 1) + (y - 1) * sectionCount];
                    }

                sectionCount = tempCount;
                mapSections = temp;
            }
        }

        /// <summary>
        /// Returns the transformed position of a unit into an index of a 1D array
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int getMapSectionIndex(float x, float y)
        {
            int posX;
            int posY;

            bool check = false;
            do
            {
                posX = (int)(transformator[x] + 0.1f);
                posY = (int)(transformator[y] + 0.1f);

                check = posX < 0 || posX >= sectionCount || posY < 0 || posY >= sectionCount;

                if (check)
                    enlargeMap();

            } while (check);

            return posX + posY * sectionCount;
        }

        /// <summary>
        /// Returns the transformed position of a unit into and index of a 1D array
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        private int getMapSectionIndex(Vector vector)
        {
            Debug.Assert(vector != null);

            return getMapSectionIndex(vector.X, vector.Y);
        }

        /// <summary>
        /// Adds or updates the array to the correct mapsection
        /// </summary>
        /// <param name="array">Units to add to this map</param>
        /// <param name="positionOffset">Position offset to update the map unit position</param>
        private void addOrUpdateUnits(MapUnit[] array, Vector positionOffset)
        {
            for (int i = 0; i < array.Length; i++)
            {
                MapUnit mapUnit = array[i];

                if (mapUnit == null)
                    break;

                mapUnit.PositionInternal = positionOffset + mapUnit.PositionInternal;

                if (mapUnit.IsOrbiting)
                    mapUnit.OrbitingCenter = positionOffset + mapUnit.OrbitingCenter;

                int currentIndex = getMapSectionIndex(mapUnit.PositionInternal);

                for (int a = 0; a < mapSections.Length; a++)
                    if (a != currentIndex && mapSections[a].Remove(mapUnit))
                        break;

                mapSections[currentIndex].AddOrUpdate(mapUnit);
            }
        }

        /// <summary>
        /// Calculates the current unit count in this map
        /// Does not take into account if units are unique
        /// </summary>
        /// <returns>Total amount if units</returns>
        private int unitCount()
        {
            // TODO: Check if this is called too often, can be a performance problem
            int count = 0;
            for (int i = 0; i < mapSections.Length; i++)
                count += mapSections[i].StillCount + mapSections[i].AgingCount + mapSections[i].PlayerCount;

            return count;
        }
        #endregion
    }
}
