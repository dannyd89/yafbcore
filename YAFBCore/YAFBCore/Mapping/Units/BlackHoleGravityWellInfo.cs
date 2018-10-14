using Flattiverse;
using System;
using System.Collections.Generic;
using System.Text;

namespace YAFBCore.Mapping.Units
{
    public class BlackHoleGravityWellInfo
    {
        public readonly float Radius;
        public readonly float Gravity;

        public BlackHoleGravityWellInfo(GravityWell gravityWell)
        {
            Radius = gravityWell.Radius;
            Gravity = gravityWell.GravityMovement;
        }
    }
}
