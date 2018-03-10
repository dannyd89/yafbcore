using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class BlackHoleMapUnit : MapUnit
	{
		private BlackHole blackHole;
	
		public BlackHoleMapUnit(Map map, BlackHole blackHole, Vector movementOffset)
			: base(map, blackHole, movementOffset)
		{
			this.blackHole = blackHole;
		}
	}
}
