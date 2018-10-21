using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class SwitchMapUnit : MapUnit
	{
		private Switch @switch;

        public bool Switched;
	
		public SwitchMapUnit(Map map, Switch @switch, Vector movementOffset)
			: base(map, @switch, movementOffset)
		{
			this.@switch = @switch;

            Switched = @switch.Switched;
		}
	}
}
