using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class HastePowerUpMapUnit : MapUnit
	{
		private HastePowerUp hastePowerUp;
	
		public HastePowerUpMapUnit(Map map, HastePowerUp hastePowerUp, Vector movementOffset)
			: base(map, hastePowerUp, movementOffset)
		{
			this.hastePowerUp = hastePowerUp;
        }

        public override int AgeMax => 5;

        public override bool IsAging => true;

        public override Mobility Mobility => Mobility.Mobile;
    }
}
