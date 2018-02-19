using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface ITargetDominationScoredMessageListener
	{
		void OnTargetDominationScoredMessage(object sender, TargetDominationScoredMessage msg);
	}
}
