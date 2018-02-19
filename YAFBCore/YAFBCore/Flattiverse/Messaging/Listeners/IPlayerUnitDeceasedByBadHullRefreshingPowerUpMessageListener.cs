using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerUnitDeceasedByBadHullRefreshingPowerUpMessageListener
	{
		void OnPlayerUnitDeceasedByBadHullRefreshingPowerUpMessage(object sender, PlayerUnitDeceasedByBadHullRefreshingPowerUpMessage msg);
	}
}
