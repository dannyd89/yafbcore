using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;
using YAFBCore.Networking;

namespace YAFBCore.Controllables
{
    public class ControllablesManager : IDisposable
    {
        /// <summary>
        /// Active session
        /// </summary>
        public readonly UniverseSession Session;

        /// <summary>
        /// Current universe group
        /// </summary>
        private readonly Flattiverse.UniverseGroup universeGroup;

        /// <summary>
        /// List of all ships
        /// </summary>
        public ImmutableArray<Ship> Ships; 

        /// <summary>
        /// Lockable object
        /// </summary>
        private object syncObj = new object();

        #region IsDisposed
        private volatile bool isDisposed;
        public bool IsDisposed => isDisposed;
        #endregion

        /// <summary>
        /// Creates a thread-safe manager which holds all controllables of a player
        /// </summary>
        /// <param name="universeGroup"></param>
        internal ControllablesManager(UniverseSession universeSession)
        {
            Session = universeSession;
            universeGroup = universeSession.UniverseGroup;

            Ships = ImmutableArray<Ship>.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="class"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Ship CreateShip(string @class, string name)
        {
            if (isDisposed)
                throw new ObjectDisposedException("ControllablesManager is already disposed");

            try
            {
                Ship ship = new Ship(Session, universeGroup.RegisterShip(@class, name));
                ship.ActiveStateChanged += ship_ActiveStateChanged;
                Ships = Ships.Add(ship);

                return ship;
            }
            catch (Flattiverse.GameException gameException)
            {
                Debug.WriteLine(gameException.ErrorNumber.ToString("X2"));
                Debug.WriteLine(gameException.Message);

                // TODO: Own exception type so we dont throw a Flattiverse Exception outside

                return null;
            }
        }

        /// <summary>
        /// Ships' active state changed to false, need to deregister it from the manager too
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ship_ActiveStateChanged(object sender, EventArgs e)
        {
            if (!isDisposed)
                TryRemoveShip(((Ship)sender).Name);
        }

        /// <summary>
        /// Tries to search for the ship in the internal list
        /// </summary>
        /// <param name="name">Name of the ship</param>
        /// <param name="ship">Object of the ship if found, else null</param>
        /// <returns>True if ship was found</returns>
        public bool TryGetShip(string name, out Ship ship)
        {
            if (isDisposed)
                throw new ObjectDisposedException("ControllablesManager is already disposed");

            var tempShips = Ships;
            for (int i = 0; i < tempShips.Length; i++)
                if (tempShips[i].Name == name)
                {
                    ship = tempShips[i];
                    return true;
                }

            ship = null;
            return true;
        }

        /// <summary>
        /// Tries to remove the ship with the given name
        /// </summary>
        /// <param name="name">Name of the ship</param>
        /// <returns>True if ship was found and removed</returns>
        public bool TryRemoveShip(string name)
        {
            if (isDisposed)
                throw new ObjectDisposedException("ControllablesManager is already disposed");

            var tempShips = Ships;
            for (int i = 0; i < tempShips.Length; i++)
                if (tempShips[i].Name == name)
                {
                    tempShips[i].Dispose();

                    Ships = tempShips.RemoveAt(i);
                    return true;
                }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (isDisposed)
                throw new ObjectDisposedException("ControllablesManager is already disposed");

            isDisposed = true;

            foreach (Ship ship in Ships)
                ship.Dispose();

            Ships = ImmutableArray<Ship>.Empty;
        }
    }
}
