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
        private MapUnit[] stillUnits = new MapUnit[InitialArraySize];

        /// <summary>
        /// Current index of the stillUnits array
        /// </summary>
        private int stillIndex = 0;

        /// <summary>
        /// Holds all the units which need to be aged in this section
        /// </summary>
        private MapUnit[] agingUnits = new MapUnit[InitialArraySize];

        /// <summary>
        /// Current index of the agingUnits array
        /// </summary>
        private int agingIndex = 0;

        /// <summary>
        /// 
        /// </summary>
        private MapUnit[] playerUnits = new MapUnit[InitialArraySize];

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
                if (!arrayContains(stillUnits, mapUnit, out index))
                    addInternal(ref stillUnits, ref stillIndex, mapUnit);
            }

            if (mapUnit.IsAging)
            {
                if (!arrayContains(agingUnits, mapUnit, out index))
                    addInternal(ref agingUnits, ref agingIndex, mapUnit);
                else
                {
                    if (agingUnits[index].Kind == mapUnit.Kind)
                        agingUnits[index].Update(mapUnit);
                    else
                        agingUnits[index] = mapUnit;
                }
            }

            if (mapUnit.Kind == UnitKind.PlayerBase 
                || mapUnit.Kind == UnitKind.PlayerDrone 
                || mapUnit.Kind == UnitKind.PlayerPlatform 
                || mapUnit.Kind == UnitKind.PlayerProbe 
                || mapUnit.Kind == UnitKind.PlayerShip)
            {
                if (!arrayContains(playerUnits, mapUnit, out index))
                    addInternal(ref playerUnits, ref playerIndex, mapUnit);
                else
                {
                    if (playerUnits[index].Kind == mapUnit.Kind)
                        playerUnits[index].Update(mapUnit);
                    else
                        playerUnits[index] = mapUnit;
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
            int length = array.Length;
            for (int i = 0; i < length; i++)
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void addInternal(ref MapUnit[] array, ref int index, MapUnit mapUnit)
        {
            int currentCapacity = array.Length;

            if (index + 1 > currentCapacity)
                enlargeArray(ref array);

            array[index++] = mapUnit;
        }
    }
}
