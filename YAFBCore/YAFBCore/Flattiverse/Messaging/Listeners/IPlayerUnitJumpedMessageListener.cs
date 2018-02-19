using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerUnitJumpedMessageListener
	{
		void OnPlayerUnitJumpedMessage(object sender, PlayerUnitJumpedMessage msg);
	}
}
