using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface ITargetDedominationStartedMessageListener
	{
		void OnTargetDedominationStartedMessage(object sender, TargetDedominationStartedMessage msg);
	}
}
