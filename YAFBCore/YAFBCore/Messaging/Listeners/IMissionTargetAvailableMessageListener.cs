using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface IMissionTargetAvailableMessageListener
	{
		void OnMissionTargetAvailableMessage(object sender, MissionTargetAvailableMessage msg);
	}
}
