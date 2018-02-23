using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Mapping.Units
{
	public class GateMapUnit : MapUnit
	{
		private Gate gate;
	
		public GateMapUnit(Map map, Gate gate, Vector movementOffset)
			: base(map, gate, movementOffset)
		{
			this.gate = gate;
		}
	}
}
