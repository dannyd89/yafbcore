using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerUnitBuildFinishedMessageListener
	{
		void OnPlayerUnitBuildFinishedMessage(object sender, PlayerUnitBuildFinishedMessage msg);
	}
}
