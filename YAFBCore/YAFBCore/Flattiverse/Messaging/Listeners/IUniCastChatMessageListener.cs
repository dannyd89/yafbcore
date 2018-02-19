using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IUniCastChatMessageListener
	{
		void OnUniCastChatMessage(object sender, UniCastChatMessage msg);
	}
}
