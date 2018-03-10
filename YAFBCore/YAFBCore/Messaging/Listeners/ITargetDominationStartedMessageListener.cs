using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface ITargetDominationStartedMessageListener
	{
		void OnTargetDominationStartedMessage(object sender, TargetDominationStartedMessage msg);
	}
}
