using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerUnitLoggedOffMessageListener
	{
		void OnPlayerUnitLoggedOffMessage(object sender, PlayerUnitLoggedOffMessage msg);
	}
}
