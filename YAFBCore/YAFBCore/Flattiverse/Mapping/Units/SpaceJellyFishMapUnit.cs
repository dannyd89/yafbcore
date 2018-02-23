using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Mapping.Units
{
	public class SpaceJellyFishMapUnit : MapUnit
	{
		private SpaceJellyFish spaceJellyFish;
	
		public SpaceJellyFishMapUnit(Map map, SpaceJellyFish spaceJellyFish, Vector movementOffset)
			: base(map, spaceJellyFish, movementOffset)
		{
			this.spaceJellyFish = spaceJellyFish;
		}
	}
}
