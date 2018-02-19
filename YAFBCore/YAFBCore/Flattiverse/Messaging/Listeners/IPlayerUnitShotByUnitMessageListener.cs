using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerUnitShotByUnitMessageListener
	{
		void OnPlayerUnitShotByUnitMessage(object sender, PlayerUnitShotByUnitMessage msg);
	}
}
