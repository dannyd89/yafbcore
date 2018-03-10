using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerUnitDeceasedByBadHullRefreshingPowerUpMessageListener
	{
		void OnPlayerUnitDeceasedByBadHullRefreshingPowerUpMessage(object sender, PlayerUnitDeceasedByBadHullRefreshingPowerUpMessage msg);
	}
}
