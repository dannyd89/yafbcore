using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerUnitHitMissionTargetMessageListener
	{
		void OnPlayerUnitHitMissionTargetMessage(object sender, PlayerUnitHitMissionTargetMessage msg);
	}
}
