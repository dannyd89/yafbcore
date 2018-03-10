using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAFBCore.Mapping.Units
{
    public class UnitOrbitingState
    {
        public int Rotation;
        public readonly int RotationInterval;
        public readonly int RotationIntervalAbsolute;
        public float Angle;
        public readonly float StartAngle;
        public readonly float Distance;

        internal UnitOrbitingState(OrbitingState orbitingState)
        {
            Rotation = orbitingState.Rotation;
            RotationInterval = orbitingState.RotationInterval;
            RotationIntervalAbsolute = orbitingState.RotationInterval < 0 ? orbitingState.RotationInterval * -1 : orbitingState.RotationInterval;
            Angle = orbitingState.Angle;
            StartAngle = orbitingState.StartAngle;
            Distance = orbitingState.Distance;
        }

        internal UnitOrbitingState(UnitOrbitingState orbitingState)
        {
            Rotation = orbitingState.Rotation;
            RotationInterval = orbitingState.RotationInterval;
            RotationIntervalAbsolute = orbitingState.RotationInterval < 0 ? orbitingState.RotationInterval * -1 : orbitingState.RotationInterval;
            Angle = orbitingState.Angle;
            StartAngle = orbitingState.StartAngle;
            Distance = orbitingState.Distance;
        }
    }
}
