using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IGateSwitchedMessageListener
	{
		void OnGateSwitchedMessage(object sender, GateSwitchedMessage msg);
	}
}
