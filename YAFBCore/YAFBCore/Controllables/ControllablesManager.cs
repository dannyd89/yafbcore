using System;
using System.Collections.Generic;
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
        private List<Ship> ships;

        // TODO: Add other controllable types

        /// <summary>
        /// Lockable object
        /// </summary>
        private object syncObj = new object();

        #region IsDisposed
        private volatile bool isDisposed;
        public bool IsDisposed => isDisposed;
        #endregion

        /// <summary>
        /// Creates a manager which holds all controllables of a player
        /// </summary>
        /// <param name="universeGroup"></param>
        internal ControllablesManager(UniverseSession universeSession)
        {
            Session = universeSession;
            universeGroup = universeSession.UniverseGroup;

            ships = new List<Ship>(16);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="class"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public Ship CreateShip(string @class, string name)
        {
            lock (syncObj)
            {
                if (isDisposed)
                    throw new ObjectDisposedException("ControllablesManager is already disposed");

                try
                {
                    Ship ship = new Ship(Session, universeGroup.RegisterShip(@class, name));
                    ship.ActiveStateChanged += ship_ActiveStateChanged;
                    ships.Add(ship);

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
        }

        /// <summary>
        /// Ships' active state changed to false, need to deregister it from the manager too
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ship_ActiveStateChanged(object sender, EventArgs e)
        {
            lock (syncObj)
            {
                if (!isDisposed)
                    TryRemoveShip(((Ship)sender).Name);
            }
        }

        /// <summary>
        /// Tries to search for the ship in the internal list
        /// </summary>
        /// <param name="name">Name of the ship</param>
        /// <param name="ship">Object of the ship if found, else null</param>
        /// <returns>True if ship was found</returns>
        public bool TryGetShip(string name, out Ship ship)
        {
            lock (syncObj)
            {
                if (isDisposed)
                    throw new ObjectDisposedException("ControllablesManager is already disposed");

                int count = ships.Count;
                for (int i = 0; i < count; i++)
                    if (ships[i].Name == name)
                    {
                        ship = ships[i];
                        return true;
                    }

                ship = null;
                return true;
            }
        }

        /// <summary>
        /// Tries to remove the ship with the given name
        /// </summary>
        /// <param name="name">Name of the ship</param>
        /// <returns>True if ship was found and removed</returns>
        public bool TryRemoveShip(string name)
        {
            lock (syncObj)
            {
                if (isDisposed)
                    throw new ObjectDisposedException("ControllablesManager is already disposed");

                int count = ships.Count;
                for (int i = 0; i < count; i++)
                    if (ships[i].Name == name)
                    {
                        ships[i].Dispose();

                        ships.RemoveAt(i);
                        return true;
                    }
                
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            lock (syncObj)
            {
                if (isDisposed)
                    throw new ObjectDisposedException("ControllablesManager is already disposed");

                isDisposed = true;

                foreach (Ship ship in ships)
                    ship.Dispose();
            }
        }
    }
}
