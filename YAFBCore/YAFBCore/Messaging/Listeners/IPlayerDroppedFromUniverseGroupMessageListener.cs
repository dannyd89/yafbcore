using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerDroppedFromUniverseGroupMessageListener
	{
		void OnPlayerDroppedFromUniverseGroupMessage(object sender, PlayerDroppedFromUniverseGroupMessage msg);
	}
}
