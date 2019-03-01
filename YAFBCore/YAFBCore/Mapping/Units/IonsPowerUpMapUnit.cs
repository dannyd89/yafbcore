using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class IonsPowerUpMapUnit : MapUnit
	{
		private IonsRefreshingPowerUp ionsPowerUp;
	
		public IonsPowerUpMapUnit(Map map, IonsRefreshingPowerUp ionsPowerUp, Vector movementOffset)
			: base(map, ionsPowerUp, movementOffset)
		{
			this.ionsPowerUp = ionsPowerUp;
        }

        public override int AgeMax => 5;

        public override bool IsAging => true;

        public override Mobility Mobility => Mobility.Mobile;
    }
}
