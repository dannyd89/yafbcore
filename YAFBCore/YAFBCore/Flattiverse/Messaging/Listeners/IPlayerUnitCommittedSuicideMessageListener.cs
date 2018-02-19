using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IPlayerUnitCommittedSuicideMessageListener
	{
		void OnPlayerUnitCommittedSuicideMessage(object sender, PlayerUnitCommittedSuicideMessage msg);
	}
}
