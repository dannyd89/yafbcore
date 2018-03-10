using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class PlayerShipMapUnit : MapUnit
	{
		private PlayerShip playerShip;

        private Controllable controllable;
	
		public PlayerShipMapUnit(Map map, PlayerShip playerShip, Vector movementOffset)
			: base(map, playerShip, movementOffset)
		{
			this.playerShip = playerShip;
		}

        public PlayerShipMapUnit(Map map, Controllable controllable, Vector movementOffset)
            : base(map, 
                   UnitKind.PlayerShip,
                   controllable.Radius,
                   controllable.Name,
                   Mobility.Mobile,
                   false,
                   false,
                   true,
                   controllable.Gravity,
                   Vector.FromNull(),
                   movementOffset)
        {
            this.controllable = controllable;
        }

        public override bool IsAging => true;

        public override int AgeMax => controllable != null ? -1 : 100;

        internal override bool Age()
        {
            PositionInternal = PositionInternal + MovementInternal;

            return base.Age();
        }
    }
}
