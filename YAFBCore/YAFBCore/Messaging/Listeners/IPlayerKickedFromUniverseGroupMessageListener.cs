using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerKickedFromUniverseGroupMessageListener
	{
		void OnPlayerKickedFromUniverseGroupMessage(object sender, PlayerKickedFromUniverseGroupMessage msg);
	}
}
