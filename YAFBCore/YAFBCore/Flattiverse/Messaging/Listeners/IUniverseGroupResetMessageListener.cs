using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IUniverseGroupResetMessageListener
	{
		void OnUniverseGroupResetMessage(object sender, UniverseGroupResetMessage msg);
	}
}
