using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class DoubleDamagePowerUpMapUnit : MapUnit
	{
		private DoubleDamagePowerUp doubleDamagePowerUp;
	
		public DoubleDamagePowerUpMapUnit(Map map, DoubleDamagePowerUp doubleDamagePowerUp, Vector movementOffset)
			: base(map, doubleDamagePowerUp, movementOffset)
		{
			this.doubleDamagePowerUp = doubleDamagePowerUp;
        }

        public override int AgeMax => 5;

        public override bool IsAging => true;

        public override Mobility Mobility => Mobility.Mobile;
    }
}
