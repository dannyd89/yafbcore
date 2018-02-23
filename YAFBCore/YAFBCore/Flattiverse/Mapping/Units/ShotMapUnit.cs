using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Mapping.Units
{
	public class ShotMapUnit : MapUnit
	{
		private Shot shot;
	
		public ShotMapUnit(Map map, Shot shot, Vector movementOffset)
			: base(map, shot, movementOffset)
		{
			this.shot = shot;
		}
	}
}
