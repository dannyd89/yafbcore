using Flattiverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAFBCore.Networking
{
    public class UniverseGroupFlowControlWrapper : IDisposable
    {
        internal readonly UniverseGroupFlowControl FlowControl;

        private bool isDisposed;

        public bool IsDisposed
        {
            get { return isDisposed; }
        }

        internal UniverseGroupFlowControlWrapper(UniverseGroupFlowControl flowControl)
        {
            FlowControl = flowControl;
        }

        /// <summary>
        /// Returns when the PreWait-Phase has ended
        /// </summary>
        /// <returns></returns>
        public TimeSpan PreWait()
        {
            return FlowControl.PreWait();
        }

        /// <summary>
        /// Returns when the Wait-Phase has ended
        /// </summary>
        /// <returns></returns>
        public TimeSpan Wait()
        {
            return FlowControl.Wait();
        }

        /// <summary>
        /// Commits this FlowControl-instance. Returns if the commit was in time
        /// </summary>
        /// <returns></returns>
        public bool Commit()
        {
            return FlowControl.Commit();
        }

        /// <summary>
        /// Dispose of the FlowControl
        /// </summary>
        public void Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;

                FlowControl.Dispose();
            }
        }
    }
}
