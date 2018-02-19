using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerUnitCollidedWithPlayerUnitMessageListener
	{
		void OnPlayerUnitCollidedWithPlayerUnitMessage(object sender, PlayerUnitCollidedWithPlayerUnitMessage msg);
	}
}
