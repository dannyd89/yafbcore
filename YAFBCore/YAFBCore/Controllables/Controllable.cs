using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using YAFBCore.Networking;

namespace YAFBCore.Controllables
{
    public abstract class Controllable : IDisposable, IEquatable<Controllable>
    {
        /// <summary>
        /// Active session
        /// </summary>
        public readonly UniverseSession Session;

        /// <summary>
        /// Flattiverse controllable
        /// </summary>
        internal readonly Flattiverse.Controllable Base;

        /// <summary>
        /// Name of controllable
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Configurator class of this controllable
        /// </summary>
        public readonly string Class;

        /// <summary>
        /// Radius of the controllable
        /// </summary>
        public readonly float Radius;

        /// <summary>
        /// The needed size of tiles for save path finding this controllable needs
        /// </summary>
        public readonly int NeededTileSize;

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
        protected bool isDisposed;

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
        public bool IsActive => Base.IsActive;

        /// <summary>
        /// States if controllable is currently alive
        /// </summary>
        public bool IsAlive => Base.IsAlive;
        #endregion

        /// <summary>
        /// Creates a controllable
        /// </summary>
        /// <param name="universeSession">Active session</param>
        /// <param name="controllable">Flattiverse controllable</param>
        internal Controllable(UniverseSession universeSession, Flattiverse.Controllable controllable)
        {
            Session = universeSession;

            this.Base = controllable;

            Name = controllable.Name;
            Class = controllable.Class;

            NeededTileSize = 2;
            Radius = controllable.Radius;
            int tempRadius = (int)(Radius * 2f + 0.1f);

            while (NeededTileSize < tempRadius)
                NeededTileSize <<= 1;

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
            if (isDisposed)
                throw new ObjectDisposedException("Controllable is already disposed");

            isDisposed = true;

            Session.RemoveFlowControl(flowControl);

            while (workerThread.ThreadState == ThreadState.Running)
                Thread.Sleep(50);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        public virtual void Queue(Commands.Command command) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Controllable other)
        {
            if (other == null)
                return false;

            return Name == other.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as Controllable);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
