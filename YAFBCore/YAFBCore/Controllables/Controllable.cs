using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using YAFBCore.Networking;

namespace YAFBCore.Controllables
{
    public abstract class Controllable : IDisposable
    {
        /// <summary>
        /// Active session
        /// </summary>
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
        protected volatile bool isDisposed;

        #region Properties
        /// <summary>
        /// Determines if this controllable THEORETICALLY can scan.
        /// The controllable still needs a scanner as a component, else it still can't scan
        /// </summary>
        public virtual bool CanScan => false;

        /// <summary>
        /// Determines if this controllable THEORETICALLY can move.
        /// The controllable still needs a engine as a component, else it still can't move
        /// </summary>
        public virtual bool CanMove => false;

        /// <summary>
        /// Determines if this controllable THEORETICALLY can shoot.
        /// The controllable still needs a weapon as a component, else it still can't shoot
        /// </summary>
        public virtual bool CanShoot => false;

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
        /// Creates a controllable
        /// </summary>
        /// <param name="universeSession">Active session</param>
        /// <param name="controllable">Flattiverse controllable</param>
        internal Controllable(UniverseSession universeSession, Flattiverse.Controllable controllable)
        {
            Session = universeSession;

            this.controllable = controllable;

            Name = controllable.Name;
            Class = controllable.Class;

            flowControl = Session.CreateFlowControl();
            workerThread = new Thread(new ThreadStart(worker));

            workerThread.Start();
        }
        
        /// <summary>
        /// Asynchronous worker function running in workerThread.
        /// The communication with the flattiverse server happens in here
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

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            isDisposed = true;

            while (workerThread.ThreadState == ThreadState.Running)
                Thread.Sleep(10);
        }
    }
}
