using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerUnitBuildStartMessageListener
	{
		void OnPlayerUnitBuildStartMessage(object sender, PlayerUnitBuildStartMessage msg);
	}
}
