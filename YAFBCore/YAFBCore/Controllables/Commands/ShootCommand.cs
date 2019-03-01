using System;
using System.Collections.Generic;
using System.Text;

namespace YAFBCore.Controllables.Commands
{
    public class ShootCommand : Command
    {
        public readonly float X;
        public readonly float Y;

        internal readonly Flattiverse.Vector Position;

        internal Flattiverse.Vector Direction;
        internal int Time;
        internal float Load;
        internal float DamageHull;
        internal float DamageShield;
        internal float DamageEnergy;
        internal List<Flattiverse.SubDirection> SubDirections;

        public override CommandType Type => CommandType.Shoot;

        public ShootCommand(float x, float y)
        {
            X = x;
            Y = y;

            Position = new Flattiverse.Vector(x, y);
        }

        internal void TempSetup(Flattiverse.Ship ship, Mapping.Units.PlayerShipMapUnit playerShipMapUnit)
        {
            Direction = Position - playerShipMapUnit.PositionInternal;
            Direction = Direction + playerShipMapUnit.MovementInternal;

            Time = (1 + (int)(Direction.Length / ship.WeaponShot.Speed.Limit * 0.99f));

            if (Time > ship.WeaponShot.Time.Limit)
                Time = (int)(ship.WeaponShot.Time.Limit * 0.99f);

            Direction.Length /= Time;

            if (Direction > ship.WeaponShot.Speed.Limit * 0.99f)
                Direction.Length = ship.WeaponShot.Speed.Limit * 0.99f;

            Load = ship.WeaponShot.Load.Limit * 0.85f;
            DamageHull = ship.WeaponShot.DamageHull.Limit * 0.95f;
            DamageShield = ship.WeaponShot.DamageShield.Limit * 0.75f;
            DamageEnergy = ship.WeaponShot.DamageEnergy.Limit * 0.75f;
        }
    }
}
