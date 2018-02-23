using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Mapping.Units
{
	public class MissionTargetMapUnit : MapUnit
	{
		private MissionTarget missionTarget;
	
		public MissionTargetMapUnit(Map map, MissionTarget missionTarget, Vector movementOffset)
			: base(map, missionTarget, movementOffset)
		{
			this.missionTarget = missionTarget;
		}
	}
}
