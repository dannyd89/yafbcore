using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Mapping.Units
{
	public class PlayerShipMapUnit : MapUnit
	{
		private PlayerShip playerShip;
	
		public PlayerShipMapUnit(Map map, PlayerShip playerShip, Vector movementOffset)
			: base(map, playerShip, movementOffset)
		{
			this.playerShip = playerShip;
		}
	}
}
