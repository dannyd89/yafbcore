using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class NebulaMapUnit : MapUnit
	{
		private Nebula nebula;
	
		public NebulaMapUnit(Map map, Nebula nebula, Vector movementOffset)
			: base(map, nebula, movementOffset)
		{
			this.nebula = nebula;
		}
	}
}
