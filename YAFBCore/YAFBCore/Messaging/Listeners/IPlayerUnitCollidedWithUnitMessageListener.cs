using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerUnitCollidedWithUnitMessageListener
	{
		void OnPlayerUnitCollidedWithUnitMessage(object sender, PlayerUnitCollidedWithUnitMessage msg);
	}
}
