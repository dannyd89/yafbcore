using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerJoinedUniverseGroupMessageListener
	{
		void OnPlayerJoinedUniverseGroupMessage(object sender, PlayerJoinedUniverseGroupMessage msg);
	}
}
