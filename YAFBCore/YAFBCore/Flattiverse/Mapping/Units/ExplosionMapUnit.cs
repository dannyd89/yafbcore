using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Mapping.Units
{
	public class ExplosionMapUnit : MapUnit
	{
		private Explosion explosion;
	
		public ExplosionMapUnit(Map map, Explosion explosion, Vector movementOffset)
			: base(map, explosion, movementOffset)
		{
			this.explosion = explosion;
		}
	}
}
