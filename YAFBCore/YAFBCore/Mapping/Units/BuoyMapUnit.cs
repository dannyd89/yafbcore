using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class BuoyMapUnit : MapUnit
	{
		private Buoy buoy;
	
		public BuoyMapUnit(Map map, Buoy buoy, Vector movementOffset)
			: base(map, buoy, movementOffset)
		{
			this.buoy = buoy;
		}
	}
}
