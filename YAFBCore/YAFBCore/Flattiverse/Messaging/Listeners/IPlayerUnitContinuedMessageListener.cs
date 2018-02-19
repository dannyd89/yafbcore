using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerUnitContinuedMessageListener
	{
		void OnPlayerUnitContinuedMessage(object sender, PlayerUnitContinuedMessage msg);
	}
}
