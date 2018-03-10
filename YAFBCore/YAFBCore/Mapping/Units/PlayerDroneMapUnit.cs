using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class PlayerDroneMapUnit : MapUnit
	{
		private PlayerDrone playerDrone;
	
		public PlayerDroneMapUnit(Map map, PlayerDrone playerDrone, Vector movementOffset)
			: base(map, playerDrone, movementOffset)
		{
			this.playerDrone = playerDrone;
		}
	}
}
