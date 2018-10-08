using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flattiverse;
using System.Collections.Immutable;

namespace YAFBCore.Networking
{
    /// <summary>
    /// Stellt die Verbindung zu Flattiverse
    /// 
    /// </summary>
    public static class ConnectionManager
    {
        /// <summary>
        /// Holds all connections made to Flattiverse
        /// </summary>
        public static ImmutableDictionary<string, Connection> Connections = ImmutableDictionary<string, Connection>.Empty;

        /// <summary>
        /// Creates a connection to Flattiverse
        /// If the connection is still
        /// </summary>
        /// <param name="email">Email of the Flattiverse account</param>
        /// <param name="password">Password of the Flattiverse account</param>
        /// <returns></returns>
        public static Connection Connect(string email, string password)
        {
            Connection connection;
            if (Connections.TryGetValue(email, out connection)
                && connection.Connector.IsConnected)
                    return connection;

            connection = Connection.From(email, password);

            if (connection != null)
                Connections = Connections.Add(email, connection);

            return connection;
        }

        /// <summary>
        /// Closes the connection of the passed account
        /// </summary>
        /// <param name="email">Email of the Flattiverse account</param>
        public static void Close(string email)
        {
            Connection connection;
            if (Connections.TryGetValue(email, out connection))
            {
                Connections = Connections.Remove(email);

                connection.Close();
            }
        }

        /// <summary>
        /// Does a benchmark of your system
        /// Is sometimes required to join specific universes
        /// </summary>
        public static void DoBenchmark()
        {
            Connector.DoBenchmark();
        }
    }
}
