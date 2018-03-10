using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IFlattiverseMessageListener
	{
		void OnFlattiverseMessage(object sender, FlattiverseMessage msg);
	}
}
