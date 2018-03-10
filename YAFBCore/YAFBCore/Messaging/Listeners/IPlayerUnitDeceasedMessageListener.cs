using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerUnitDeceasedMessageListener
	{
		void OnPlayerUnitDeceasedMessage(object sender, PlayerUnitDeceasedMessage msg);
	}
}
