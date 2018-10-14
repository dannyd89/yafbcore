using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class BlackHoleMapUnit : MapUnit
	{
		private BlackHole blackHole;

        public readonly BlackHoleGravityWellInfo[] GravityWellInfos;
	
		public BlackHoleMapUnit(Map map, BlackHole blackHole, Vector movementOffset)
			: base(map, blackHole, movementOffset)
		{
			this.blackHole = blackHole;

            var gravityWells = blackHole.GravityWells;
            GravityWellInfos = new BlackHoleGravityWellInfo[gravityWells.Count];

            for (int i = 0; i < GravityWellInfos.Length; i++)
                GravityWellInfos[i] = new BlackHoleGravityWellInfo(gravityWells[i]);
		}
	}
}
