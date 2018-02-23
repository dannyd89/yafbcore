

using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Mapping.Units
{
	public class SunMapUnit : MapUnit
	{
		private Sun sun;
	
		public SunMapUnit(Map map, Sun sun, Vector movementOffset)
			: base(map, sun, movementOffset)
		{
			this.sun = sun;
		}
	}
}
