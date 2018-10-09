using System;
using System.Collections.Generic;
using System.Text;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
    public class SunCoronaInfo
    {
        public readonly float Radius;
        public readonly float Energy;
        public readonly float Particles;
        public readonly float Ions;

        internal SunCoronaInfo(Corona corona)
        {
            Radius = corona.Radius;
            Energy = corona.Energy;
            Particles = corona.Particles;
            Ions = corona.Ions;
        }
    }
}
