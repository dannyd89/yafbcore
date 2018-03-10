using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class ParticlesPowerUpMapUnit : MapUnit
	{
		private ParticlesRefreshingPowerUp particlesPowerUp;
	
		public ParticlesPowerUpMapUnit(Map map, ParticlesRefreshingPowerUp particlesPowerUp, Vector movementOffset)
			: base(map, particlesPowerUp, movementOffset)
		{
			this.particlesPowerUp = particlesPowerUp;
		}
	}
}
