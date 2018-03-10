using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IPlayerUnitCommittedSuicideMessageListener
	{
		void OnPlayerUnitCommittedSuicideMessage(object sender, PlayerUnitCommittedSuicideMessage msg);
	}
}
