using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class MissionTargetMapUnit : MapUnit
	{
		private MissionTarget missionTarget;
	
		public MissionTargetMapUnit(Map map, MissionTarget missionTarget, Vector movementOffset)
			: base(map, missionTarget, movementOffset)
		{
			this.missionTarget = missionTarget;
		}

        public override bool IsAging => mobility == Mobility.Steady;

        public int SequenceNumber => missionTarget.SequenceNumber;

        public ReadOnlyCollection<Vector> Hints => missionTarget.Hints;
    }
}
