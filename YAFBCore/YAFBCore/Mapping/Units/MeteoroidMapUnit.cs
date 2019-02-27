using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class MeteoroidMapUnit : MapUnit
	{
		private Meteoroid meteoroid;
	
		public MeteoroidMapUnit(Map map, Meteoroid meteoroid, Vector movementOffset)
			: base(map, meteoroid, movementOffset)
		{
			this.meteoroid = meteoroid;
		}

        public override bool IsAging => mobility == Mobility.Steady;

        internal override bool Age()
        {
            Console.WriteLine("Aging steady meteoroid");

            return base.Age();
        }
    }
}
