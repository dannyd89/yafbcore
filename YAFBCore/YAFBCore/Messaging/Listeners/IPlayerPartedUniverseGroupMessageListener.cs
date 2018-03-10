using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerPartedUniverseGroupMessageListener
	{
		void OnPlayerPartedUniverseGroupMessage(object sender, PlayerPartedUniverseGroupMessage msg);
	}
}
