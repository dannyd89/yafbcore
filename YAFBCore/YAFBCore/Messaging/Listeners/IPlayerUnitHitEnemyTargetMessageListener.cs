using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerUnitHitEnemyTargetMessageListener
	{
		void OnPlayerUnitHitEnemyTargetMessage(object sender, PlayerUnitHitEnemyTargetMessage msg);
	}
}
