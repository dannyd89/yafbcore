using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerUnitDeceasedMessageListener
	{
		void OnPlayerUnitDeceasedMessage(object sender, PlayerUnitDeceasedMessage msg);
	}
}
