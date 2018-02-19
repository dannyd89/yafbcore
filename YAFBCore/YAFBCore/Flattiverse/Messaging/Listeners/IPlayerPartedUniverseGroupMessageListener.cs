using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerPartedUniverseGroupMessageListener
	{
		void OnPlayerPartedUniverseGroupMessage(object sender, PlayerPartedUniverseGroupMessage msg);
	}
}
