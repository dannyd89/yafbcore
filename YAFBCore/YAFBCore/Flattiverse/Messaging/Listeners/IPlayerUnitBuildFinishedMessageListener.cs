using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerUnitBuildFinishedMessageListener
	{
		void OnPlayerUnitBuildFinishedMessage(object sender, PlayerUnitBuildFinishedMessage msg);
	}
}
