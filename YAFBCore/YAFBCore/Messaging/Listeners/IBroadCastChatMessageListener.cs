using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IBroadCastChatMessageListener
	{
		void OnBroadCastChatMessage(object sender, BroadCastChatMessage msg);
	}
}
