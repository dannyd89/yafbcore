using Flattiverse;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using YAFBCore.Flattiverse.Mapping.Units;
using YAFBCore.Utils.Mathematics;

namespace YAFBCore.Flattiverse.Mapping
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

        /// <summary>
        /// Holds all the still units in this section
        /// </summary>
        internal MapUnit[] StillUnits = new MapUnit[InitialArraySize];

        /// <summary>
        /// Current index of the stillUnits array
        /// </summary>
        private int stillIndex = 0;

        /// <summary>
        /// Holds all the units which need to be aged in this section
        /// </summary>
        internal MapUnit[] AgingUnits = new MapUnit[InitialArraySize];

        /// <summary>
        /// Current index of the agingUnits array
        /// </summary>
        private int agingIndex = 0;

        /// <summary>
        /// 
        /// </summary>
        internal MapUnit[] PlayerUnits = new MapUnit[InitialArraySize];

        /// <summary>
        /// Current index of the playerUnits array
        /// </summary>
        private int playerIndex = 0;

        /// <summary>
        /// Creates a map section
        /// </summary>
        /// <param name="parent">Parent of this section</param>
        public MapSection(Map parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapUnit"></param>
        public void AddOrUpdate(MapUnit mapUnit)
        {
            int index;
            if (mapUnit.Mobility == Mobility.Still)
            {
                if (!arrayContains(StillUnits, mapUnit, out index))
                    addInternal(ref StillUnits, ref stillIndex, mapUnit);
            }

            if (mapUnit.IsAging)
            {
                if (!arrayContains(AgingUnits, mapUnit, out index))
                    addInternal(ref AgingUnits, ref agingIndex, mapUnit);
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
                    addInternal(ref PlayerUnits, ref playerIndex, mapUnit);
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
        /// Searches in the array for the passed unit
        /// </summary>
        /// <param name="array">Array to search in</param>
        /// <param name="mapUnit">Unit to be searched</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        /// 
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
    }
}
