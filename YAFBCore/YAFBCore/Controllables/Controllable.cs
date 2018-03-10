using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using YAFBCore.Networking;

namespace YAFBCore.Controllables
{
    public abstract class Controllable : IDisposable
    {
        public readonly UniverseSession Session;

        /// <summary>
        /// Flattiverse controllable
        /// </summary>
        protected readonly Flattiverse.Controllable controllable;

        /// <summary>
        /// Name of controllable
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Configurator class of this controllable
        /// </summary>
        public readonly string Class;

        /// <summary>
        /// Flow control for the current universe group
        /// </summary>
        protected UniverseGroupFlowControlWrapper flowControl;

        /// <summary>
        /// Main worker thread
        /// </summary>
        protected Thread workerThread;

        /// <summary>
        /// 
        /// </summary>
        protected volatile bool isRunning;

        #region Properties
        /// <summary>
        /// States if controllable is currently active in the universe
        /// If false, needs to join again
        /// </summary>
        public bool IsActive => controllable.IsActive;

        /// <summary>
        /// States if controllable is currently alive
        /// </summary>
        public bool IsAlive => controllable.IsAlive;
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllable"></param>
        internal Controllable(UniverseSession universeSession, Flattiverse.Controllable controllable)
        {
            Session = universeSession;

            this.controllable = controllable;

            Name = controllable.Name;
            Class = controllable.Class;

            flowControl = Session.CreateFlowControl();
            workerThread = new Thread(new ThreadStart(worker));
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract void worker();

        /// <summary>
        /// 
        /// </summary>
        protected virtual void scan() { }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void move() { }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void shoot() { }

        public virtual void Dispose()
        {
            isRunning = false;

            workerThread = null;
        }
    }
}
