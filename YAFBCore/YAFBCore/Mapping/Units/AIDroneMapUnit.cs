using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class AIDroneMapUnit : MapUnit
	{
		private AIDrone aIDrone;
	
		public AIDroneMapUnit(Map map, AIDrone aIDrone, Vector movementOffset)
			: base(map, aIDrone, movementOffset)
		{
			this.aIDrone = aIDrone;
		}
	}
}
