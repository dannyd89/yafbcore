using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface ITargetDominationFinishedMessageListener
	{
		void OnTargetDominationFinishedMessage(object sender, TargetDominationFinishedMessage msg);
	}
}
