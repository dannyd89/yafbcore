using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class SpaceJellyFishSlimeMapUnit : MapUnit
	{
		private SpaceJellyFishSlime spaceJellyFishSlime;
	
		public SpaceJellyFishSlimeMapUnit(Map map, SpaceJellyFishSlime spaceJellyFishSlime, Vector movementOffset)
			: base(map, spaceJellyFishSlime, movementOffset)
		{
			this.spaceJellyFishSlime = spaceJellyFishSlime;
		}
	}
}
