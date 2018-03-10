using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class WormHoleMapUnit : MapUnit
	{
		private WormHole wormHole;
	
		public WormHoleMapUnit(Map map, WormHole wormHole, Vector movementOffset)
			: base(map, wormHole, movementOffset)
		{
			this.wormHole = wormHole;
		}
	}
}
