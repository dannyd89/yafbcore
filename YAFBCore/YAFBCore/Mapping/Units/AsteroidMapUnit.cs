using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class AsteroidMapUnit : MapUnit
	{
		private Asteroid asteroid;
	
		public AsteroidMapUnit(Map map, Asteroid asteroid, Vector movementOffset)
			: base(map, asteroid, movementOffset)
		{
			this.asteroid = asteroid;
		}
	}
}
