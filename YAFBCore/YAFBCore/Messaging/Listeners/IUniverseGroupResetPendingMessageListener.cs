using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IUniverseGroupResetPendingMessageListener
	{
		void OnUniverseGroupResetPendingMessage(object sender, UniverseGroupResetPendingMessage msg);
	}
}
