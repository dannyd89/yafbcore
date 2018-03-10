using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class PlayerPlatformMapUnit : MapUnit
	{
		private PlayerPlatform playerPlatform;
	
		public PlayerPlatformMapUnit(Map map, PlayerPlatform playerPlatform, Vector movementOffset)
			: base(map, playerPlatform, movementOffset)
		{
			this.playerPlatform = playerPlatform;
		}
	}
}
