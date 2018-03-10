using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerUnitHitOwnTargetMessageListener
	{
		void OnPlayerUnitHitOwnTargetMessage(object sender, PlayerUnitHitOwnTargetMessage msg);
	}
}
