using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IGameMessageListener
	{
		void OnGameMessage(object sender, GameMessage msg);
	}
}
