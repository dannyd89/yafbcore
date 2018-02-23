using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Mapping.Units
{
	public class IonsPowerUpMapUnit : MapUnit
	{
		private IonsRefreshingPowerUp ionsPowerUp;
	
		public IonsPowerUpMapUnit(Map map, IonsRefreshingPowerUp ionsPowerUp, Vector movementOffset)
			: base(map, ionsPowerUp, movementOffset)
		{
			this.ionsPowerUp = ionsPowerUp;
		}
	}
}
