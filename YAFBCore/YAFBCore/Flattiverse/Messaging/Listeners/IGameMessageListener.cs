using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IGameMessageListener
	{
		void OnGameMessage(object sender, GameMessage msg);
	}
}
