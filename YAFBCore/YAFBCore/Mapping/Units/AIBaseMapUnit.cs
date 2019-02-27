using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class AIBaseMapUnit : MapUnit
	{
		private AIBase aIBase;
	
		public AIBaseMapUnit(Map map, AIBase aIBase, Vector movementOffset)
			: base(map, aIBase, movementOffset)
		{
			this.aIBase = aIBase;
		}
    }
}
