using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerDroppedFromUniverseGroupMessageListener
	{
		void OnPlayerDroppedFromUniverseGroupMessage(object sender, PlayerDroppedFromUniverseGroupMessage msg);
	}
}
