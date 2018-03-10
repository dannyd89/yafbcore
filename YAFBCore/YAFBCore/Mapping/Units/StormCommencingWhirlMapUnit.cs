using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class StormCommencingWhirlMapUnit : MapUnit
	{
		private StormCommencingWhirl stormCommencingWhirl;
	
		public StormCommencingWhirlMapUnit(Map map, StormCommencingWhirl stormCommencingWhirl, Vector movementOffset)
			: base(map, stormCommencingWhirl, movementOffset)
		{
			this.stormCommencingWhirl = stormCommencingWhirl;
		}
	}
}
