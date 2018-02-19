using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerUnitHitMissionTargetMessageListener
	{
		void OnPlayerUnitHitMissionTargetMessage(object sender, PlayerUnitHitMissionTargetMessage msg);
	}
}
