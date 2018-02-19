using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IFlattiverseMessageListener
	{
		void OnFlattiverseMessage(object sender, FlattiverseMessage msg);
	}
}
