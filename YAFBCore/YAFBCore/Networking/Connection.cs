using Flattiverse;
using YAFBCore.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YAFBCore.Networking
{
    /// <summary>
    /// This class stores the connection information to Flattiverse for an account
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// Email of the account who established this connection
        /// </summary>
        public readonly string Email;

        #region Events
        public event EventHandler Disconnected;
        #endregion

        #region Connector
        private Connector connector;

        internal Connector Connector
        {
            get { return connector; }
        }
        #endregion

        #region Controllables
        public UniversalHolder<Controllable> Controllables
        {
            get { return connector.Controllables; }
        }
        #endregion

        #region Crystals
        public UniversalHolder<CrystalCargoItem> Crystals
        {
            get { return connector.Crystals; }
        }
        #endregion

        #region Players
        public UniversalHolder<Player> Players
        {
            get { return connector.Players; }
        }
        #endregion

        #region Player
        /// <summary>
        /// Player object of this connection
        /// </summary>
        public Player Player
        {
            get { return connector.Player; }
        }
        #endregion

        #region UniverseGroups
        public UniversalHolder<UniverseGroup> UniverseGroups
        {
            get { return connector.UniverseGroups; }
        }
        #endregion

        #region UniverseSession
        private UniverseSession session;

        /// <summary>
        /// Returns the current session
        /// </summary>
        public UniverseSession Session => session;
        #endregion

        #region MessageManager
        private MessageManager messageManager;

        public MessageManager MessageManager => messageManager;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        private Connection(string email)
        {
            Email = email;
        }

        /// <summary>
        /// Creates a connection with the passed parameters
        /// </summary>
        /// <param name="email">Email of the account to use</param>
        /// <param name="password">Password of the account</param>
        /// <returns>A new connection to flattiverse</returns>
        internal static Connection From(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("Value is null");

            Connection connection = new Connection(email);
            connection.connector = new Connector(email, password);

            foreach (var changeLogEntry in connection.connector.Changelog)
            {
                foreach (string change in changeLogEntry.Changes)
                    Console.WriteLine(change);
                Console.WriteLine(changeLogEntry.Description);
            }

            connection.messageManager = new MessageManager(connection);

            return connection;
        }

        /// <summary>
        /// Closes the connection to the Flattiverse server
        /// </summary>
        internal void Close()
        {
            if (connector.IsConnected)
            {
                Disconnected?.Invoke(this, EventArgs.Empty);

                Leave();

                connector.Close();
            }
        }

        /// <summary>
        /// Joins the passed universe group 
        /// </summary>
        /// <param name="universeGroup">The desired universe group to join</param>
        /// <param name="name">The nickname which will be used in the universe</param>
        /// <param name="team">Team to join in the universe</param>
        /// <returns>Returns a session of the universe</returns>
        public UniverseSession Join(UniverseGroup universeGroup, string name, Team team)
        {
            if (!connector.IsConnected)
                throw new ObjectDisposedException("Connection to Flattiverse was closed");

            if (universeGroup == null || string.IsNullOrWhiteSpace(name) || team == null)
                throw new ArgumentNullException("Value is null");

            if (session != null)
                throw new InvalidOperationException("Close the current session first before joining a new one");

            messageManager.UniverseGroup = universeGroup;
            session = new UniverseSession(this, universeGroup, name, team);

            return session;
        }

        /// <summary>
        /// Leaves the current joined session
        /// </summary>
        public void Leave()
        {
            if (session == null)
                throw new InvalidOperationException("There is no session currently joined which can be left");

            messageManager.Dispose();
            session.Dispose();
            session = null;
        }
    }
}
