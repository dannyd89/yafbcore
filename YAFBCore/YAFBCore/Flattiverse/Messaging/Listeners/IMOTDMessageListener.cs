using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IMOTDMessageListener
	{
		void OnMOTDMessage(object sender, MOTDMessage msg);
	}
}
