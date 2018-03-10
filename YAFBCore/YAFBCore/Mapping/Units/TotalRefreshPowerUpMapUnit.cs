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
	}
}
