using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IBroadCastChatMessageListener
	{
		void OnBroadCastChatMessage(object sender, BroadCastChatMessage msg);
	}
}
