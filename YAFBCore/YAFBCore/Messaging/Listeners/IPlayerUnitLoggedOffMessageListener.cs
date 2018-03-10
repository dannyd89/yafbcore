using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerUnitLoggedOffMessageListener
	{
		void OnPlayerUnitLoggedOffMessage(object sender, PlayerUnitLoggedOffMessage msg);
	}
}
