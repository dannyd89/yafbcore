﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flattiverse;
using YAFBCore.Flattiverse.Mapping.Units;
using YAFBCore.Flattiverse.Extensions;
using YAFBCore.Utils;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using YAFBCore.Utils.Mathematics;

namespace YAFBCore.Flattiverse.Mapping
{
    public class Map : IDisposable, IComparable<Map>
    {
        private static long counter = 0;

        /// <summary>
        /// Id of this object
        /// </summary>
        public readonly long Id;

        /// <summary>
        /// Defines the width and height of a section
        /// </summary>
        internal const int SectionSize = 4096;

        /// <summary>
        /// The universe this map belongs to
        /// </summary>
        public readonly Universe Universe;

        /// <summary>
        /// Holding all known still units in this map for faster merging
        /// </summary>
        private Dictionary<string, MapUnit> stillUnits = new Dictionary<string, MapUnit>();

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
        /// States whether the map is locked or not
        /// </summary>
        private volatile bool isLocked = false;

        /// <summary>
        /// 
        /// </summary>
        private volatile bool isDisposed;

        /// <summary>
        /// True when Dispose() was called
        /// </summary>
        public bool IsDisposed => isDisposed;

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
        public static Map Create(Controllable creator, List<Unit> units)
        {
            Vector movementOffset = Vector.FromNull();
            foreach (Unit unit in units)
                if (unit.Kind != UnitKind.Explosion
                    && unit.Mobility == Mobility.Still)
                {
                    movementOffset = unit.Movement;
                    break;
                }

            if (movementOffset.IsZeroVector())
                return null;

            Universe universe = creator.Universe;
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

            mapUnit = MapUnitFactory.GetMapUnit(map, creator, movementOffset);
            map.mapSections[map.getMapSectionIndex(mapUnit.PositionInternal)].AddOrUpdate(mapUnit);

            return map;
        }
        #endregion

        #region Public functions
        /// <summary>
        /// <para>Locks map for current thread to read/edit.</para>
        /// Execute before calling any functions who want to read/edit the map, or if you want a consistent state of the map for a specific call
        /// </summary>
        public void BeginLock()
        {
            if (isDisposed)
                throw new InvalidOperationException("Map is already disposed");

            lockMapEvent.WaitOne();
            isLocked = true;
        }

        /// <summary>
        /// Releases the lock for other threads to edit
        /// </summary>
        public void EndLock()
        {
            if (isDisposed)
                throw new InvalidOperationException("Map is already disposed");

            isLocked = false;
            lockMapEvent.Set();
        }

        /// <summary>
        /// Tries to merge the two maps with each other
        /// </summary>
        /// <param name="other">The other map to merge with</param>
        /// <returns>Returns true if merge was successful</returns>
        public bool Merge(Map other)
        {
            if (isDisposed)
                throw new InvalidOperationException("Map is already disposed");

            if (!isLocked || !other.isLocked)
                throw new InvalidOperationException("Please acquire a lock on both maps before trying to merge them");

            Vector positionOffset = null;

            MapUnit mapUnit;
            foreach (KeyValuePair<string, MapUnit> unitKvp in other.stillUnits)
                if (stillUnits.TryGetValue(unitKvp.Key, out mapUnit))
                {
                    positionOffset = unitKvp.Value.PositionInternal - mapUnit.PositionInternal;
                    break;
                }

            // No reference point found between both maps
            if (positionOffset == null)
                return false;
            
            for (int otherIndex = 0; otherIndex < other.mapSections.Length; otherIndex++)
            {
                MapSection mapSection = other.mapSections[otherIndex];
                MapUnit[] array = mapSection.StillUnits;
                addOrUpdateUnits(array, positionOffset);

                for (int i = 0; i < mapSection.StillUnits.Length; i++)
                {
                    if (array[i] == null)
                        break;

                    if (!stillUnits.TryGetValue(array[i].Name, out mapUnit))
                    {
                        array[i].PositionInternal = positionOffset + array[i].PositionInternal;

                        if (array[i].IsOrbiting)
                            array[i].OrbitingCenter = positionOffset + array[i].OrbitingCenter;

                        stillUnits.Add(array[i].Name, array[i]);
                    }
                }

                addOrUpdateUnits(mapSection.AgingUnits, positionOffset);
                addOrUpdateUnits(mapSection.PlayerUnits, positionOffset);
            }

            return true;
        }

        /// <summary>
        /// Ages the map for one tick.
        /// </summary>
        public void Age()
        {
            if (isDisposed)
                throw new InvalidOperationException("Map is already disposed");

            if (!isLocked)
                throw new InvalidOperationException("Please acquire a lock on this map");

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
                            mapSection.AgingUnits[x] = null;
                    }

                    mapSection.Sort(MapSectionSortType.AgingUnits);
                }
            }
        }

        /// <summary>
        /// Gets the units which are visible in the passed viewport
        /// </summary>
        /// <param name="viewport">Viewport which describes the area the game is currently looking at. It's highly advised to span the passed viewport a bit larger than the actual viewport.</param>
        /// <returns>A list of units which are contained in the viewport</returns>
        public List<MapUnit> GetUnits(RectangleF viewport)
        {
            if (isDisposed)
                throw new InvalidOperationException("Map is already disposed");

            if (!isLocked)
                throw new InvalidOperationException("Please acquire a lock on this map");

            List<MapUnit> mapUnits = new List<MapUnit>(300);
            for (int i = getMapSectionIndex(viewport.Left, viewport.Top); i < getMapSectionIndex(viewport.Bottom, viewport.Right); i++)
            {
                MapSection mapSection = mapSections[i];

                if (mapSection.Bounds.Intersects(viewport))
                {
                    mapUnits.AddRange(mapSection.StillUnits);

                    for (int x = 0; x < mapSection.AgingCount; x++)
                        if (!mapUnits.Contains(mapSection.AgingUnits[x]))
                            mapUnits.Add(mapSection.AgingUnits[x]);

                    for (int x = 0; x < mapSection.PlayerCount; x++)
                        if (!mapUnits.Contains(mapSection.PlayerUnits[x]))
                            mapUnits.Add(mapSection.PlayerUnits[x]);
                }
            }

            return mapUnits;
        }

        /// <summary>
        /// Used for sorting maps.
        /// <para>Sort Type: DESC</para>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Map other)
        {
            if (isDisposed)
                throw new InvalidOperationException("Map is already disposed");

            if (!isLocked)
                throw new InvalidOperationException("Please acquire a lock on this map");

            if (other == null)
                return -1;

            // Sort it desc
            return -unitCount().CompareTo(other.unitCount());
        }

        /// <summary>
        /// Disposes the map
        /// </summary>
        public void Dispose()
        {
            if (isDisposed)
                throw new InvalidOperationException("Map is already disposed");

            mapSections = null;
            stillUnits = null;

            lockMapEvent.Dispose();
        }

        /// <summary>
        /// Prints the map into the console
        /// </summary>
        [Conditional("DEBUG")]
        public void DebugPrint()
        {
            if (isDisposed)
                throw new InvalidOperationException("Map is already disposed");

            if (!isLocked)
                throw new InvalidOperationException("Please acquire a lock on this map");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("-- Still Units --                                 ");

            foreach (KeyValuePair<string, MapUnit> unitKvp in stillUnits)
                sb.AppendLine(unitKvp.Value.ToString());

            for (int i = 0; i < mapSections.Length; i++)
            {
                if (mapSections[i].AgingCount > 0)
                {
                    sb.Append("-- Printing section with Index:                              ");
                    sb.AppendLine(i.ToString());

                    sb.AppendLine("\t---- Aging Units -----                              ");
                    for (int x = 0; x < mapSections[i].AgingUnits.Length; x++)
                    {
                        if (mapSections[i].AgingUnits[x] == null)
                            break;

                        sb.AppendLine(mapSections[i].AgingUnits[x].ToString() + "                                ");
                    }
                }
            }

            Console.WriteLine(sb.ToString());
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
                        if (y == 0 || y == tempCount - 1 && x == 0 || x == tempCount - 1)
                            temp[x * y] = new MapSection(this, new RectangleF(transformator.Rev(x), transformator.Rev(y), SectionSize, SectionSize));
                        else
                            temp[x * y] = mapSections[(x - 1) * (y - 1)];
                    }

                sectionCount = tempCount;
                mapSections = temp;
            }
        }

        /// <summary>
        /// Returns the transformed position of a unit into and index of a 1D array
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int getMapSectionIndex(float x, float y)
        {
            float posX;
            float posY;

            bool check = false;
            do
            {
                posX = transformator[x];
                posY = transformator[y];

                check = posX < 0f || posX > sectionCount || posY < 0f || posY > sectionCount;

                if (check)
                    enlargeMap();

            } while (check);

            return (int)posX + (int)posY * sectionCount;
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
        /// 
        /// </summary>
        /// <param name="array"></param>
        private void addOrUpdateUnits(MapUnit[] array, Vector positionOffset)
        {
            for (int i = 0; i < array.Length; i++)
            {
                MapUnit mapUnit = array[i];

                if (mapUnit == null)
                    break;

                mapUnit.PositionInternal = mapUnit.PositionInternal + positionOffset;

                if (mapUnit.IsOrbiting)
                    mapUnit.OrbitingCenter = mapUnit.OrbitingCenter + positionOffset;

                mapSections[getMapSectionIndex(mapUnit.PositionInternal)].AddOrUpdate(mapUnit);
            }
        }

        /// <summary>
        /// Calculates the current unit count in this map
        /// Does not take into account if units are unique
        /// </summary>
        /// <returns>Total amount if units</returns>
        private int unitCount()
        {
            int count = 0;
            for (int i = 0; i < mapSections.Length; i++)
                count += mapSections[i].StillCount + mapSections[i].AgingCount + mapSections[i].PlayerCount;

            return count;
        }
        #endregion
    }
}
