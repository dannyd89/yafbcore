using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface ITargetDominationStartedMessageListener
	{
		void OnTargetDominationStartedMessage(object sender, TargetDominationStartedMessage msg);
	}
}
