using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerUnitBuildMessageListener
	{
		void OnPlayerUnitBuildMessage(object sender, PlayerUnitBuildMessage msg);
	}
}
