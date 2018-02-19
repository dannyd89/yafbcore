using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IChatMessageListener
	{
		void OnChatMessage(object sender, ChatMessage msg);
	}
}
