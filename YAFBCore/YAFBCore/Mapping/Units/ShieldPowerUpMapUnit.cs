using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class ShieldPowerUpMapUnit : MapUnit
	{
		private ShieldRefreshingPowerUp shieldPowerUp;
	
		public ShieldPowerUpMapUnit(Map map, ShieldRefreshingPowerUp shieldPowerUp, Vector movementOffset)
			: base(map, shieldPowerUp, movementOffset)
		{
			this.shieldPowerUp = shieldPowerUp;
        }

        public override int AgeMax => 5;

        public override bool IsAging => true;

        public override Mobility Mobility => Mobility.Mobile;
    }
}
