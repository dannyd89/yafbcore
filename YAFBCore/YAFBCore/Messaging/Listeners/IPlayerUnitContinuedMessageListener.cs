using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerUnitContinuedMessageListener
	{
		void OnPlayerUnitContinuedMessage(object sender, PlayerUnitContinuedMessage msg);
	}
}
