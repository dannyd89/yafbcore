using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Mapping.Units
{
	public class MoonMapUnit : MapUnit
	{
		private Moon moon;
	
		public MoonMapUnit(Map map, Moon moon, Vector movementOffset)
			: base(map, moon, movementOffset)
		{
			this.moon = moon;
		}
	}
}
