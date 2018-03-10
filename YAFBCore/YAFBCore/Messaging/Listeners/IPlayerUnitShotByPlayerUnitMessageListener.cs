using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerUnitShotByPlayerUnitMessageListener
	{
		void OnPlayerUnitShotByPlayerUnitMessage(object sender, PlayerUnitShotByPlayerUnitMessage msg);
	}
}
