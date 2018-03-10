

using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IBinaryChatMessageListener
	{
		void OnBinaryChatMessage(object sender, BinaryChatMessage msg);
	}
}
