using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerUnitShotByUnitMessageListener
	{
		void OnPlayerUnitShotByUnitMessage(object sender, PlayerUnitShotByUnitMessage msg);
	}
}
