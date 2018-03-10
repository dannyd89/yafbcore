using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerUnitBuildCanceledMessageListener
	{
		void OnPlayerUnitBuildCanceledMessage(object sender, PlayerUnitBuildCanceledMessage msg);
	}
}
