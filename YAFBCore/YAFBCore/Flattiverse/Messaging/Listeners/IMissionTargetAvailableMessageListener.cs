using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface IMissionTargetAvailableMessageListener
	{
		void OnMissionTargetAvailableMessage(object sender, MissionTargetAvailableMessage msg);
	}
}
