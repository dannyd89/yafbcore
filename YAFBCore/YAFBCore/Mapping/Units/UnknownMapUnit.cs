using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class UnknownMapUnit : MapUnit
	{
		private Unit unit;
	
		public UnknownMapUnit(Map map, Unit unit, Vector movementOffset)
			: base(map, unit, movementOffset)
		{
			this.unit = unit;
		}
	}
}
