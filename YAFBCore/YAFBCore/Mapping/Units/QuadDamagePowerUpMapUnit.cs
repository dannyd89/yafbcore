using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class QuadDamagePowerUpMapUnit : MapUnit
	{
		private QuadDamagePowerUp quadDamagePowerUp;
	
		public QuadDamagePowerUpMapUnit(Map map, QuadDamagePowerUp quadDamagePowerUp, Vector movementOffset)
			: base(map, quadDamagePowerUp, movementOffset)
		{
			this.quadDamagePowerUp = quadDamagePowerUp;
        }

        public override int AgeMax => 5;

        public override bool IsAging => true;

        public override Mobility Mobility => Mobility.Mobile;
    }
}
