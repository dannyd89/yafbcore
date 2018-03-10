using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerUnitJumpedMessageListener
	{
		void OnPlayerUnitJumpedMessage(object sender, PlayerUnitJumpedMessage msg);
	}
}
