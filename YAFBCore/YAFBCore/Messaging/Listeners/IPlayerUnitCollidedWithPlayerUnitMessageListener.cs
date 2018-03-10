using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerUnitCollidedWithPlayerUnitMessageListener
	{
		void OnPlayerUnitCollidedWithPlayerUnitMessage(object sender, PlayerUnitCollidedWithPlayerUnitMessage msg);
	}
}
