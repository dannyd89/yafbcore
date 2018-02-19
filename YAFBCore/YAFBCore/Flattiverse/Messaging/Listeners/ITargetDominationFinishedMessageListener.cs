using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface ITargetDominationFinishedMessageListener
	{
		void OnTargetDominationFinishedMessage(object sender, TargetDominationFinishedMessage msg);
	}
}
