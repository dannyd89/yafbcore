using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IChatMessageListener
	{
		void OnChatMessage(object sender, ChatMessage msg);
	}
}
