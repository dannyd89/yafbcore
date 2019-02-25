using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class ShotMapUnit : MapUnit
	{
		private Shot shot;
	
		public ShotMapUnit(Map map, Shot shot, Vector movementOffset)
			: base(map, shot, movementOffset)
		{
			this.shot = shot;
		}

        public override bool IsAging => true;

        public override int AgeMax => 5;

        internal override bool Age()
        {
            PositionInternal += MovementInternal;

            return base.Age();
        }
    }
}
