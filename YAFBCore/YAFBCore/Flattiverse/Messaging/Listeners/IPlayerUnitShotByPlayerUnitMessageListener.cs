using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerUnitShotByPlayerUnitMessageListener
	{
		void OnPlayerUnitShotByPlayerUnitMessage(object sender, PlayerUnitShotByPlayerUnitMessage msg);
	}
}
