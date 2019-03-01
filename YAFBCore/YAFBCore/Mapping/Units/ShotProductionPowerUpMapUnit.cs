using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class ShotProductionPowerUpMapUnit : MapUnit
	{
		private ShotProductionRefreshingPowerUp shotProductionPowerUp;
	
		public ShotProductionPowerUpMapUnit(Map map, ShotProductionRefreshingPowerUp shotProductionPowerUp, Vector movementOffset)
			: base(map, shotProductionPowerUp, movementOffset)
		{
			this.shotProductionPowerUp = shotProductionPowerUp;
        }

        public override int AgeMax => 5;

        public override bool IsAging => true;

        public override Mobility Mobility => Mobility.Mobile;
    }
}
