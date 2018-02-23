using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Mapping.Units
{
	public class SpawnerMapUnit : MapUnit
	{
		private Spawner spawner;
	
		public SpawnerMapUnit(Map map, Spawner spawner, Vector movementOffset)
			: base(map, spawner, movementOffset)
		{
			this.spawner = spawner;
		}
	}
}
