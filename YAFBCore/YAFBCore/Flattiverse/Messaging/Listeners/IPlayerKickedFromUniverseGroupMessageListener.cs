using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerKickedFromUniverseGroupMessageListener
	{
		void OnPlayerKickedFromUniverseGroupMessage(object sender, PlayerKickedFromUniverseGroupMessage msg);
	}
}
