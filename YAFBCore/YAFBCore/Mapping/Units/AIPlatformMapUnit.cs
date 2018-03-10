using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class AIPlatformMapUnit : MapUnit
	{
		private AIPlatform aIPlatform;
	
		public AIPlatformMapUnit(Map map, AIPlatform aIPlatform, Vector movementOffset)
			: base(map, aIPlatform, movementOffset)
		{
			this.aIPlatform = aIPlatform;
		}
	}
}
