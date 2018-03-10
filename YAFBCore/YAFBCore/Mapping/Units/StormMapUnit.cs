using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class StormMapUnit : MapUnit
	{
		private Storm storm;
	
		public StormMapUnit(Map map, Storm storm, Vector movementOffset)
			: base(map, storm, movementOffset)
		{
			this.storm = storm;
		}
	}
}
