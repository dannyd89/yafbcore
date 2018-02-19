using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IGateSwitchedMessageListener
	{
		void OnGateSwitchedMessage(object sender, GateSwitchedMessage msg);
	}
}
