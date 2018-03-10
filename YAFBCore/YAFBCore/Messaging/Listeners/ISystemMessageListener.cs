using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface ISystemMessageListener
	{
		void OnSystemMessage(object sender, SystemMessage msg);
	}
}
