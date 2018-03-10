using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class AIProbeMapUnit : MapUnit
	{
		private AIProbe aIProbe;
	
		public AIProbeMapUnit(Map map, AIProbe aIProbe, Vector movementOffset)
			: base(map, aIProbe, movementOffset)
		{
			this.aIProbe = aIProbe;
		}
	}
}
