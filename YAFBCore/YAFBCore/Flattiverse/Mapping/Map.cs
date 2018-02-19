using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flattiverse;
using YAFBCore.Flattiverse.Mapping.Units;

namespace YAFBCore.Flattiverse.Mapping
{
    public class Map
    {
        private static long counter = 0;
        public readonly long Id;

        /// <summary>
        /// Defines the width and height of a section
        /// </summary>
        private const int SECTION_SIZE = 512;

        /// <summary>
        /// Holding all known still units in this map for faster merging
        /// </summary>
        private Dictionary<string, MapUnit> stillUnits = new Dictionary<string, MapUnit>();

        /// <summary>
        /// Creates a new map based upon a recent scan
        /// </summary>
        /// <param name="creator">Unit which scanned the passed units</param>
        /// <param name="units">Units which were scanned</param>
        public Map(Controllable creator, List<Unit> units)
        {
            Id = counter++;

            Vector movementOffset = Vector.FromNull();
            foreach (Unit unit in units)
                if (unit.Kind != UnitKind.Explosion
                    && unit.Mobility == Mobility.Still)
                {
                    movementOffset = unit.Movement;
                    break;
                }



        }
    }
}
