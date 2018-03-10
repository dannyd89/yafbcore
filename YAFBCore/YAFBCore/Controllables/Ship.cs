using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using YAFBCore.Networking;

namespace YAFBCore.Controllables
{
    public class Ship : Controllable
    {
        #region Events
        /// <summary>
        /// Is called when ship isActive state is set to false
        /// We want to reregister this ship then in the manager
        /// </summary>
        public event EventHandler ShipActiveStateChanged;
        #endregion

        /// <summary>
        /// Flattiverse ship
        /// </summary>
        private Flattiverse.Ship ship;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ship"></param>
        internal Ship(UniverseSession universeSession, Flattiverse.Ship ship) 
            : base(universeSession, ship)
        {
            this.ship = ship;
        }

        /// <summary>
        /// Continues the ship if possible
        /// </summary>
        /// <returns>True if continue was successful</returns>
        public bool TryContinue()
        {
            try
            {
                if (!ship.IsActive)
                {
                    ShipActiveStateChanged?.Invoke(this, EventArgs.Empty);
                    return false;
                }

                if (!ship.IsAlive)
                    ship.Continue();

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

                return false;
            }
        }

        protected override void worker()
        {
            throw new NotImplementedException();
        }
    }
}
