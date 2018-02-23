using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flattiverse;
using YAFBCore.Flattiverse.Mapping.Units;
using YAFBCore.Flattiverse.Extensions;

namespace YAFBCore.Flattiverse.Mapping
{
    public class Map
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
        private int sectionExtensionCounter = 0;

        /// <summary>
        /// Private constructor to set Id
        /// </summary>
        private Map()
        {
            Id = counter++;

#warning TODO: Setup default sections
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

            string currentPlayerName = creator.Universe.Connector.Player.Name;

            Map map = new Map();

            MapUnit mapUnit;
            foreach (Unit unit in units)
            {
                if (unit.Kind == UnitKind.PlayerBase || unit.Kind == UnitKind.PlayerDrone || unit.Kind == UnitKind.PlayerPlatform || unit.Kind == UnitKind.PlayerProbe || unit.Kind == UnitKind.PlayerShip
                    && ((PlayerUnit)unit).Player.Name == currentPlayerName)
                    continue;

                mapUnit = MapUnitFactory.GetMapUnit(map, unit, movementOffset);

#warning TODO: Add unit to section
            }

            // Sections mit 3 + 2 ^ x erweitern, dadurch bleibt immer eine Mitte

            /*
            int size = 4096;
            int count = 3; //+ (int)Math.Pow(2, 16);

            float x = 5000f;
            float y = 0f;

            int index = (int)((x + (count / 2f) * size) / size) + (int)((y + (count / 2f) * size) / size) * count;
            */

            return map;
        }

        private int getMapSectionIndex(float x, float y, int sectionCount)
        {
#warning TODO: Hier muss noch ein Check rein ob X/Y außerhalb der Section-Bounds sind, wenn ja müssen die Sections erweitert werden


            return (int)(((x + (sectionCount / 2f) * SectionSize) / SectionSize) + ((y + (sectionCount / 2f) * SectionSize) / SectionSize) * sectionCount);
        }
    }
}
