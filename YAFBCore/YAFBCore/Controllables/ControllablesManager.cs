using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using YAFBCore.Networking;

namespace YAFBCore.Controllables
{
    public class ControllablesManager
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
        /// List of all controllables sorted by name
        /// </summary>
        private Dictionary<string, Controllable> controllables;

        /// <summary>
        /// Lockable object
        /// </summary>
        private object syncObj = new object();

        /// <summary>
        /// Creates a manager which holds all controllables of a player
        /// </summary>
        /// <param name="universeGroup"></param>
        internal ControllablesManager(UniverseSession universeSession, Flattiverse.UniverseGroup universeGroup)
        {
            this.universeGroup = universeGroup;

            controllables = new Dictionary<string, Controllable>();
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
                if (controllables.TryGetValue(name, out Controllable controllable) && controllable is Ship)
                    return (Ship)controllable;

                try
                {
                    Ship ship = new Ship(universeGroup.RegisterShip(@class, name));
                    controllables.Add(name, ship);

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
    }
}
