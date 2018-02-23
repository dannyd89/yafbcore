using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Mapping.Units
{
	public class QuadDamagePowerUpMapUnit : MapUnit
	{
		private QuadDamagePowerUp quadDamagePowerUp;
	
		public QuadDamagePowerUpMapUnit(Map map, QuadDamagePowerUp quadDamagePowerUp, Vector movementOffset)
			: base(map, quadDamagePowerUp, movementOffset)
		{
			this.quadDamagePowerUp = quadDamagePowerUp;
		}
	}
}
