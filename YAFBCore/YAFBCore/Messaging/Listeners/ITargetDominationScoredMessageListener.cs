using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface ITargetDominationScoredMessageListener
	{
		void OnTargetDominationScoredMessage(object sender, TargetDominationScoredMessage msg);
	}
}
