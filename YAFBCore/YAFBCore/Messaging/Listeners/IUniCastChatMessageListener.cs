using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IUniCastChatMessageListener
	{
		void OnUniCastChatMessage(object sender, UniCastChatMessage msg);
	}
}
