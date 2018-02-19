

using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IBinaryChatMessageListener
	{
		void OnBinaryChatMessage(object sender, BinaryChatMessage msg);
	}
}
