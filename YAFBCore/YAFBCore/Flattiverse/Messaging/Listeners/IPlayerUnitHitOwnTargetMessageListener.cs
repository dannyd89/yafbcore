using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerUnitHitOwnTargetMessageListener
	{
		void OnPlayerUnitHitOwnTargetMessage(object sender, PlayerUnitHitOwnTargetMessage msg);
	}
}
