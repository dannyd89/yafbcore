using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface ITargetDedominationStartedMessageListener
	{
		void OnTargetDedominationStartedMessage(object sender, TargetDedominationStartedMessage msg);
	}
}
