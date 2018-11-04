using Flattiverse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using YAFBCore.Mapping.Units;
using YAFBCore.Utils.Mathematics;

namespace YAFBCore.Mapping
{
    /// <summary>
    /// Describes a section in a map
    /// A map is divided in sections for faster path finding etc
    /// </summary>
    internal class MapSection
    {
        /// <summary>
        /// Taken from array.cs of the .NET framework
        /// </summary>
        private const int MaxArrayLength = 0X7FEFFFFF;

        /// <summary>
        /// 
        /// </summary>
        private const int InitialArraySize = 64;

        /// <summary>
        /// Parent of this section
        /// </summary>
        public readonly Map Parent;

        #region Bounds
        /// <summary>
        /// Bounds of this section
        /// </summary>
        public readonly RectangleF Bounds;

        public float Left => Bounds.Left;
        public float Top => Bounds.Top;
        public float Right => Bounds.Right;
        public float Bottom => Bounds.Bottom;
        #endregion

        /// <summary>
        /// Holds all the still units in this section
        /// </summary>
        public MapUnit[] StillUnits = new MapUnit[InitialArraySize];

        /// <summary>
        /// Current count of the stillUnits array
        /// </summary>
        private int stillCount;

        /// <summary>
        /// Current count of still units in this section
        /// </summary>
        public int StillCount => stillCount;

        /// <summary>
        /// Holds all the units which need to be aged in this section
        /// </summary>
        public MapUnit[] AgingUnits = new MapUnit[InitialArraySize];

        /// <summary>
        /// Current count of the agingUnits array
        /// </summary>
        private int agingCount;

        /// <summary>
        /// Current count of aging units in this section
        /// </summary>
        public int AgingCount => agingCount;

        /// <summary>
        /// Holds all the player units in this section
        /// </summary>
        public MapUnit[] PlayerUnits = new MapUnit[InitialArraySize];

        /// <summary>
        /// Current count of the playerUnits array
        /// </summary>
        private int playerCount;

        /// <summary>
        /// Current count of player units in this section
        /// </summary>
        public int PlayerCount => playerCount;

        /// <summary>
        /// Raster list for each size
        /// </summary>
        private Dictionary<int, MapSectionRaster> rasterList = new Dictionary<int, MapSectionRaster>();

        /// <summary>
        /// Creates a map section
        /// </summary>
        /// <param name="parent">Parent of this section</param>
        public MapSection(Map parent, RectangleF bounds)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Bounds = bounds;
        }

        /// <summary>
        /// Adds or update the given unit
        /// </summary>
        /// <param name="mapUnit"></param>
        public void AddOrUpdate(MapUnit mapUnit)
        {
            int index;
            if (mapUnit.Mobility == Mobility.Still)
            {
                if (!arrayContains(StillUnits, mapUnit, out index))
                    addInternal(ref StillUnits, ref stillCount, mapUnit);
            }

            if (mapUnit.IsAging)
            {
                if (!arrayContains(AgingUnits, mapUnit, out index))
                    addInternal(ref AgingUnits, ref agingCount, mapUnit);
                else
                {
                    if (AgingUnits[index].Kind == mapUnit.Kind)
                        AgingUnits[index].Update(mapUnit);
                    else
                        AgingUnits[index] = mapUnit;
                }
            }

            if (mapUnit.Kind == UnitKind.PlayerShip // Check first cause most used
                || mapUnit.Kind == UnitKind.PlayerBase 
                || mapUnit.Kind == UnitKind.PlayerDrone 
                || mapUnit.Kind == UnitKind.PlayerPlatform 
                || mapUnit.Kind == UnitKind.PlayerProbe)
            {
                if (!arrayContains(PlayerUnits, mapUnit, out index))
                    addInternal(ref PlayerUnits, ref playerCount, mapUnit);
                else
                {
                    if (PlayerUnits[index].Kind == mapUnit.Kind)
                        PlayerUnits[index].Update(mapUnit);
                    else
                        PlayerUnits[index] = mapUnit;
                }
            }
        }

        /// <summary>
        /// Sorts any array 
        /// </summary>
        public void Sort(MapSectionSortType sortType)
        {
            int sortValue = (int)sortType;

            if ((sortValue & 0x0001) == 0x0001)
            {
                Array.Sort(StillUnits, 0, stillCount, RadiusComparer.Default);
            }

            if ((sortValue & 0x0010) == 0x0010)
            {
                Array.Sort(AgingUnits, 0, agingCount, AgeComparer.Default);

                for (int i = 0; i < AgingUnits.Length; i++)
                {
                    if (i == 0)
                        Debug.Assert(AgingUnits[i] != null);

                    if (AgingUnits[i] == null)
                    {
                        agingCount = i;
                        break;
                    }
                }
            }

            //if ((sortValue & 0x0100) == 0x0100)
            //{
            //    // TODO: PlayerUnits sorter
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tileSize"></param>
        public void Rasterize(int tileSize)
        {
            rasterList[tileSize] = MapSectionRasterizer.Rasterize(this, tileSize);
        }

        /// <summary>
        /// Searches in the array for the passed unit
        /// </summary>
        /// <param name="array">Array to search in</param>
        /// <param name="mapUnit">Unit to be searched</param>
        /// <returns></returns>
        private static bool arrayContains(MapUnit[] array, MapUnit mapUnit, out int index)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null)
                    break;

                if (array[i].Name == mapUnit.Name)
                {
                    index = i;
                    return true;
                }
            }

            index = -1;
            return false;
        }

        /// <summary>
        /// Enlarges the passed array by double the capacity
        /// </summary>
        /// <param name="array"></param>
        private static void enlargeArray(ref MapUnit[] array)
        {
            int currentCapacity = array.Length;
            int newCapacity = currentCapacity * 2;

            newCapacity = newCapacity > MaxArrayLength ? MaxArrayLength : newCapacity;

            MapUnit[] temp = new MapUnit[newCapacity];

            Array.Copy(array, 0, temp, 0, currentCapacity);

            array = temp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <param name="mapUnit"></param>
        private static void addInternal(ref MapUnit[] array, ref int index, MapUnit mapUnit)
        {
            if (index > array.Length - 1)
                enlargeArray(ref array);

            array[index++] = mapUnit;
        }

        /// <summary>
        /// Comparer to sort units by their radius.
        /// Sort Type: DESC
        /// </summary>
        private class RadiusComparer : IComparer<MapUnit>
        {
            public static readonly RadiusComparer Default = new RadiusComparer();

            private bool desc;

            public RadiusComparer(bool desc = true)
            {
                this.desc = desc;
            }

            public int Compare(MapUnit x, MapUnit y)
            {
                if (x == null && y != null)
                    return 1;
                else if (x != null && y == null)
                    return -1;
                else if (x == null && y == null)
                    return 0;

                int val = x.RadiusInternal.CompareTo(y.RadiusInternal);

                return desc ? -val : val;
            }
        }

        /// <summary>
        /// Comparer to sort units by their age.
        /// Sort Type: ASC
        /// </summary>
        private class AgeComparer : IComparer<MapUnit>
        {
            public static readonly AgeComparer Default = new AgeComparer();

            public int Compare(MapUnit x, MapUnit y)
            {
                if (x == null && y != null)
                    return 1;
                else if (x != null && y == null)
                    return -1;
                else if (x == null && y == null)
                    return 0;

                return x.AgeInternal.CompareTo(y.AgeInternal);
            }
        }
    }
}
