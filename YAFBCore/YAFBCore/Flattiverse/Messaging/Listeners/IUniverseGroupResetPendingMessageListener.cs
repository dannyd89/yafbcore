using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IUniverseGroupResetPendingMessageListener
	{
		void OnUniverseGroupResetPendingMessage(object sender, UniverseGroupResetPendingMessage msg);
	}
}
