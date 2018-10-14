using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class GateMapUnit : MapUnit
	{
		private Gate gate;

        public bool Switched;

        /// <summary>
        /// Switch needs to have the same color to switch this gate
        /// </summary>
        public float R;
        public float G;
        public float B;
	
		public GateMapUnit(Map map, Gate gate, Vector movementOffset)
			: base(map, gate, movementOffset)
		{
			this.gate = gate;

            Switched = gate.Switched;

            R = gate.R;
            G = gate.G;
            B = gate.B;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapUnit"></param>
        internal override void Update(MapUnit mapUnit)
        {
            base.Update(mapUnit);

            GateMapUnit gateMapUnit = (GateMapUnit)mapUnit;

            Switched = gateMapUnit.Switched;

            R = gateMapUnit.R;
            G = gateMapUnit.G;
            B = gateMapUnit.B;
        }
    }
}
