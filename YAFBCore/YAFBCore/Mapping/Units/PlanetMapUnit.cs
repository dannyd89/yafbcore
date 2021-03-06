using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class PlanetMapUnit : MapUnit
	{
		private Planet planet;
	
		public PlanetMapUnit(Map map, Planet planet, Vector movementOffset)
			: base(map, planet, movementOffset)
		{
			this.planet = planet;
		}

        /// <summary>
        /// 
        /// </summary>
        public override int AgeMax => -1;

        /// <summary>
        /// 
        /// </summary>
        public override bool IsAging => isOrbiting;
    }
}
