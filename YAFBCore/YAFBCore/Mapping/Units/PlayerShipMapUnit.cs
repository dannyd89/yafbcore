using System;
using Flattiverse;

namespace YAFBCore.Mapping.Units
{
	public class PlayerShipMapUnit : MapUnit
	{
		private PlayerShip playerShip;

        private Controllable controllable;
	
        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="playerShip"></param>
        /// <param name="movementOffset"></param>
		public PlayerShipMapUnit(Map map, PlayerShip playerShip, Vector movementOffset)
			: base(map, playerShip, movementOffset)
		{
			this.playerShip = playerShip;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="controllable"></param>
        /// <param name="movementOffset"></param>
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

        /// <summary>
        /// 
        /// </summary>
        public bool IsOwnShip => controllable != null;

        /// <summary>
        /// 
        /// </summary>
        public override bool IsAging => true;

        /// <summary>
        /// 
        /// </summary>
        public override int AgeMax => controllable != null ? -1 : 100;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal override bool Age()
        {
            if (!IsOwnShip)
                PositionInternal = PositionInternal + MovementInternal;

            return base.Age();
        }
    }
}
