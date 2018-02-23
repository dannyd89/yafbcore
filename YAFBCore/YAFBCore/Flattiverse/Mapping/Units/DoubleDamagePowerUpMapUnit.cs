using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Mapping.Units
{
	public class DoubleDamagePowerUpMapUnit : MapUnit
	{
		private DoubleDamagePowerUp doubleDamagePowerUp;
	
		public DoubleDamagePowerUpMapUnit(Map map, DoubleDamagePowerUp doubleDamagePowerUp, Vector movementOffset)
			: base(map, doubleDamagePowerUp, movementOffset)
		{
			this.doubleDamagePowerUp = doubleDamagePowerUp;
		}
	}
}
