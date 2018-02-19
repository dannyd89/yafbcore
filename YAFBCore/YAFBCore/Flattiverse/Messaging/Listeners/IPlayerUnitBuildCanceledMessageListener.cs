using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerUnitBuildCanceledMessageListener
	{
		void OnPlayerUnitBuildCanceledMessage(object sender, PlayerUnitBuildCanceledMessage msg);
	}
}
