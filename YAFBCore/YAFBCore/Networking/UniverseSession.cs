using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAFBCore.Controllables;
using YAFBCore.Mapping;

namespace YAFBCore.Networking
{
    public class UniverseSession : IDisposable
    {
        /// <summary>
        /// Internal id counter
        /// </summary>
        private static long counter = 0;

        /// <summary>
        /// Id of this session
        /// </summary>
        public readonly long Id;

        /// <summary>
        /// Active connection
        /// </summary>
        internal readonly Connection Connection;

        /// <summary>
        /// Use this manager to create and manage all your controllables
        /// </summary>
        public readonly ControllablesManager ControllablesManager;

        /// <summary>
        /// Use this manager to get units from the current stored maps
        /// </summary>
        public readonly MapManager MapManager;

        /// <summary>
        /// Active universe group
        /// </summary>
        public readonly Flattiverse.UniverseGroup UniverseGroup;

        /// <summary>
        /// Name of the active player
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// The chosen team
        /// </summary>
        public readonly Flattiverse.Team Team;

        /// <summary>
        /// List of all active flow controls
        /// </summary>
        private List<UniverseGroupFlowControlWrapper> flowControls;

        /// <summary>
        /// 
        /// </summary>
        private object syncObject = new object();

        #region IsDisposed
        private volatile bool isDisposed;

        public bool IsDisposed => isDisposed;
        #endregion

        /// <summary>
        /// A universe session to manage all data for this universe
        /// </summary>
        /// <param name="parent">The parent connection to join this session</param>
        /// <param name="universeGroup">The universe group to join</param>
        /// <param name="name">The name to use to join this session</param>
        /// <param name="team">The team to join</param>
        /// <param name="clan">The clan name to use</param>
        /// <param name="password">The password of the universe group</param>
        internal UniverseSession(Connection parent, Flattiverse.UniverseGroup universeGroup, string name, Flattiverse.Team team, string clan = null, string password = null)
        {
            Id = counter++;

            Connection = parent;

            UniverseGroup = universeGroup;
            Name = name;
            Team = team;

            if (universeGroup.PasswordRequired)
                universeGroup.Join(name, team, clan, password);
            else
                universeGroup.Join(name, team);

            flowControls = new List<UniverseGroupFlowControlWrapper>();

            ControllablesManager = new ControllablesManager(this);
            MapManager = new MapManager(this);
        }

        /// <summary>
        /// Creates a flow control for this universe
        /// Stores flow control into a list
        /// All flow controls will be disposed if this session is disposed
        /// </summary>
        /// <returns></returns>
        public UniverseGroupFlowControlWrapper CreateFlowControl()
        {
            lock (syncObject)
            {
                if (isDisposed)
                    throw new ObjectDisposedException("UniverseSession is already disposed");

                UniverseGroupFlowControlWrapper flowControl = new UniverseGroupFlowControlWrapper(UniverseGroup.GetNewFlowControl());

                flowControls.Add(flowControl);

                return flowControl;
            }
        }

        /// <summary>
        /// Removes the flow control from the session
        /// </summary>
        /// <param name="flowControlWrapper">The flow control to remove</param>
        public void RemoveFlowControl(UniverseGroupFlowControlWrapper flowControlWrapper)
        {
            lock (syncObject)
            {
                if (isDisposed)
                    throw new ObjectDisposedException("UniverseSession is already disposed");

                int count = flowControls.Count;

                for (int i = 0; i < count; i++)
                    if (flowControls[i].Equals(flowControlWrapper))
                    {
                        flowControls.RemoveAt(i);
                        flowControlWrapper.Dispose();

                        break;
                    }
            }
        }

        /// <summary>
        /// Disposes all information and leaves the universe
        /// </summary>
        public void Dispose()
        {
            lock (syncObject)
            {
                if (isDisposed)
                    throw new ObjectDisposedException("UniverseSession is already disposed");

                MapManager.Dispose();
                ControllablesManager.Dispose();

                foreach (var flowControl in flowControls)
                    flowControl.Dispose();

                flowControls = null;

                try
                {
                    var player = UniverseGroup.Players[Connection.Connector.Player.Name];

                    while (player.ControllableInfos.List.Count > 0)
                    {
                        Console.WriteLine("Still active units: " + player.ControllableInfos.List.Count);

                        System.Threading.Thread.Sleep(100);
                    }
                }
                catch { }

                UniverseGroup.Part();

                isDisposed = true;
            }
        }
    }
}
