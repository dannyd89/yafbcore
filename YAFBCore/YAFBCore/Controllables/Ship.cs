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
        public event EventHandler ActiveStateChanged;
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
                    ActiveStateChanged?.Invoke(this, EventArgs.Empty);
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

        /// <see cref="Controllable.worker"/>
        protected override void worker()
        {
            while (!isDisposed)
            {
                try
                {
                    flowControl.FlowControl.PreWait();

                    scan();

                    move();

                    shoot();

                    flowControl.FlowControl.Commit();
                }
                catch (Exception ex)
                {
                    if (isDisposed)
                        return;

                    Debug.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void scan()
        {
            base.scan();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void move()
        {
            base.move();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void shoot()
        {
            base.shoot();
        }

        public override void Dispose()
        {
            base.Dispose();

            try
            {
                // We try to close the ship if it's still active
                ship.Close();
            }
            catch { }
        }
    }
}
