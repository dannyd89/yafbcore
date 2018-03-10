using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerUnitBuildMessageListener
	{
		void OnPlayerUnitBuildMessage(object sender, PlayerUnitBuildMessage msg);
	}
}
