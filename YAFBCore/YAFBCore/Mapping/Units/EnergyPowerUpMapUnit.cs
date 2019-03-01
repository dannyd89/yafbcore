using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class EnergyPowerUpMapUnit : MapUnit
	{
		private EnergyRefreshingPowerUp energyPowerUp;
	
		public EnergyPowerUpMapUnit(Map map, EnergyRefreshingPowerUp energyPowerUp, Vector movementOffset)
			: base(map, energyPowerUp, movementOffset)
		{
			this.energyPowerUp = energyPowerUp;
        }

        public override int AgeMax => 5;

        public override bool IsAging => true;

        public override Mobility Mobility => Mobility.Mobile;
    }
}
