using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAFBCore.Flattiverse.Networking
{
    public class UniverseSession : IDisposable
    {
        private readonly Connection Parent;

        private readonly UniverseGroup UniverseGroup;
        private readonly string Name;
        private readonly Team Team;

        private List<UniverseGroupFlowControlWrapper> flowControls;

        private object syncObject = new object();

        #region IsDisposed
        private volatile bool isDisposed;

        public bool IsDisposed
        {
            get { return isDisposed; }
        }
        #endregion

        /// <summary>
        /// A universe session to manage all data for this universe
        /// </summary>
        /// <param name="parent">The parent connection to join this session</param>
        /// <param name="universeGroup">The universe group to join</param>
        /// <param name="name">The name to use to join this session</param>
        /// <param name="team">The team to join</param>
        internal UniverseSession(Connection parent, UniverseGroup universeGroup, string name, Team team)
        {
            Parent = parent;

            UniverseGroup = universeGroup;
            Name = name;
            Team = team;

            universeGroup.Join(name, team);

            flowControls = new List<UniverseGroupFlowControlWrapper>();
        }

        /// <summary>
        /// A universe session to manage all data for this universe
        /// </summary>
        /// <param name="parent">The parent connection to join this session</param>
        /// <param name="universeGroup">The universe group to join</param>
        /// <param name="name">The name to use to join this session</param>
        /// <param name="team">The team to join</param>
        /// <param name="clan">The clan name to use</param>
        /// <param name="password">The password of the universe group</param>
        internal UniverseSession(Connection parent, UniverseGroup universeGroup, string name, Team team, string clan, string password)
        {
            Parent = parent;

            UniverseGroup = universeGroup;
            Name = name;
            Team = team;

            universeGroup.Join(name, team, clan, password);

            flowControls = new List<UniverseGroupFlowControlWrapper>();
        }

        /// <summary>
        /// Creates a flow control for this universe
        /// Stores flow control into a list
        /// All flow controls will be disposed if this session is disposed
        /// </summary>
        /// <returns></returns>
        public UniverseGroupFlowControlWrapper CreateFlowControl()
        {
            if (!isDisposed)
                lock (syncObject)
                {
                    UniverseGroupFlowControlWrapper flowControl = new UniverseGroupFlowControlWrapper(UniverseGroup.GetNewFlowControl());
                
                    flowControls.Add(flowControl);

                    return flowControl;
                }

            throw new InvalidOperationException("Can't create flow control on a disposed universe session");
        }

        /// <summary>
        /// Removes the flow control from the session
        /// </summary>
        /// <param name="flowControlWrapper">The flow control to remove</param>
        public void RemoveFlowControl(UniverseGroupFlowControlWrapper flowControlWrapper)
        {
            lock (syncObject)
            {
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
            isDisposed = true;

            lock (syncObject)
            {
                foreach (var flowControl in flowControls)
                    flowControl.Dispose();

                flowControls = null;

                UniverseGroup.Part();
            }
        }
    }
}
