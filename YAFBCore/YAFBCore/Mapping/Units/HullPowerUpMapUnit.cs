using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class HullPowerUpMapUnit : MapUnit
	{
		private HullRefreshingPowerUp hullPowerUp;
	
		public HullPowerUpMapUnit(Map map, HullRefreshingPowerUp hullPowerUp, Vector movementOffset)
			: base(map, hullPowerUp, movementOffset)
		{
			this.hullPowerUp = hullPowerUp;
        }

        public override int AgeMax => 5;

        public override bool IsAging => true;

        public override Mobility Mobility => Mobility.Mobile;
    }
}
