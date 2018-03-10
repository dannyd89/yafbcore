using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IMOTDMessageListener
	{
		void OnMOTDMessage(object sender, MOTDMessage msg);
	}
}
