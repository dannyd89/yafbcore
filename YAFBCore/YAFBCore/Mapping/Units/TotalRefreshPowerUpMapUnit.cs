using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class TotalRefreshPowerUpMapUnit : MapUnit
	{
		private TotalRefreshingPowerUp totalRefreshPowerUp;
	
		public TotalRefreshPowerUpMapUnit(Map map, TotalRefreshingPowerUp totalRefreshPowerUp, Vector movementOffset)
			: base(map, totalRefreshPowerUp, movementOffset)
		{
			this.totalRefreshPowerUp = totalRefreshPowerUp;
        }

        public override int AgeMax => 5;

        public override bool IsAging => true;

        public override Mobility Mobility => Mobility.Mobile;
    }
}
