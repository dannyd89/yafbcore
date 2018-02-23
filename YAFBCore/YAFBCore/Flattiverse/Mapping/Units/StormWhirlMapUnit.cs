using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Mapping.Units
{
	public class StormWhirlMapUnit : MapUnit
	{
		private StormWhirl stormWhirl;
	
		public StormWhirlMapUnit(Map map, StormWhirl stormWhirl, Vector movementOffset)
			: base(map, stormWhirl, movementOffset)
		{
			this.stormWhirl = stormWhirl;
		}
	}
}
