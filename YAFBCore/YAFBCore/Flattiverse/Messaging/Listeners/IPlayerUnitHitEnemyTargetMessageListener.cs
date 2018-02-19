using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerUnitHitEnemyTargetMessageListener
	{
		void OnPlayerUnitHitEnemyTargetMessage(object sender, PlayerUnitHitEnemyTargetMessage msg);
	}
}
