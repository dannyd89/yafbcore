using FlattiBase.Ships;
using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlattiBase.Commands
{
    public class ShootCommand : Command
    {
        public readonly Vector Direction;
        public readonly int Time;
        public readonly float Load;
        public readonly float DamageHull;
        public readonly float DamageShield;
        public readonly float DamageEnergy;
        public readonly List<SubDirection> SubDirections;

        public ShootCommand(ShipBase owner, Vector direction, int time, float load, float damageHull, float damageShield, float damageEnergy)
            : base (owner)
        {
            Direction = direction;
            Time = time;
            DamageHull = damageHull;
            DamageShield = damageShield;
            DamageEnergy = damageEnergy;
            SubDirections = null;
        }

        public ShootCommand(ShipBase owner, Vector direction, int time, float load, float damageHull, float damageShield, float damageEnergy, List<SubDirection> subDirections)
            : base(owner)
        {
            Direction = direction;
            Time = time;
            Load = load;
            DamageHull = damageHull;
            DamageShield = damageShield;
            DamageEnergy = damageEnergy;
            SubDirections = subDirections;
        }
    }
}
