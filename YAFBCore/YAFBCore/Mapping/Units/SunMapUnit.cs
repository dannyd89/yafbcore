

using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class SunMapUnit : MapUnit
	{
		private Sun sun;

        public readonly SunCoronaInfo[] CoronaInfos;
	
		public SunMapUnit(Map map, Sun sun, Vector movementOffset)
			: base(map, sun, movementOffset)
		{
			this.sun = sun;

            CoronaInfos = new SunCoronaInfo[sun.Coronas.Count];

            for (int i = 0; i < CoronaInfos.Length; i++)
                CoronaInfos[i] = new SunCoronaInfo(sun.Coronas[i]);
		}
	}
}
