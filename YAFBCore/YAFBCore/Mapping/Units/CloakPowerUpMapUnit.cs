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

        public override int AgeMax => 5;

        public override bool IsAging => true;

        public override Mobility Mobility => Mobility.Mobile;
    }
}
