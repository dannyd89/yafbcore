using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IUniverseGroupResetMessageListener
	{
		void OnUniverseGroupResetMessage(object sender, UniverseGroupResetMessage msg);
	}
}
