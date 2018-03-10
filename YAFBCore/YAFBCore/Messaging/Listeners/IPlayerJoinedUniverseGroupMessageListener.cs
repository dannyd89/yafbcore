using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerJoinedUniverseGroupMessageListener
	{
		void OnPlayerJoinedUniverseGroupMessage(object sender, PlayerJoinedUniverseGroupMessage msg);
	}
}
