using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class AIShipMapUnit : MapUnit
	{
		private AIShip aIShip;
	
		public AIShipMapUnit(Map map, AIShip aIShip, Vector movementOffset)
			: base(map, aIShip, movementOffset)
		{
			this.aIShip = aIShip;
		}
	}
}
