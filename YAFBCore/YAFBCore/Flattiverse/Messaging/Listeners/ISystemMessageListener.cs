using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface ISystemMessageListener
	{
		void OnSystemMessage(object sender, SystemMessage msg);
	}
}
