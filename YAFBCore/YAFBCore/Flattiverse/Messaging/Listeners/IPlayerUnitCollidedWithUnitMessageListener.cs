using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerUnitCollidedWithUnitMessageListener
	{
		void OnPlayerUnitCollidedWithUnitMessage(object sender, PlayerUnitCollidedWithUnitMessage msg);
	}
}
