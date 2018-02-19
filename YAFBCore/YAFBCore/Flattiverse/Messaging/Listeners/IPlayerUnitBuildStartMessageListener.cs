using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerUnitBuildStartMessageListener
	{
		void OnPlayerUnitBuildStartMessage(object sender, PlayerUnitBuildStartMessage msg);
	}
}
