using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class CloakPowerUpMapUnit : MapUnit
	{
		private CloakPowerUp cloakPowerUp;
	
		public CloakPowerUpMapUnit(Map map, CloakPowerUp cloakPowerUp, Vector movementOffset)
			: base(map, cloakPowerUp, movementOffset)
		{
			this.cloakPowerUp = cloakPowerUp;
		}
	}
}
