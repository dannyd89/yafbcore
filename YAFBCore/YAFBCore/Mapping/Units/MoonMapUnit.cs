using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class MoonMapUnit : MapUnit
	{
		private Moon moon;
	
		public MoonMapUnit(Map map, Moon moon, Vector movementOffset)
			: base(map, moon, movementOffset)
		{
			this.moon = moon;
		}

        public override bool IsAging => mobility == Mobility.Steady;
    }
}
