using System;
using Flattiverse;

namespace YAFBCore.Flattiverse.Messaging.Listeners
{
	public interface ITeamCastChatMessageListener
	{
		void OnTeamCastChatMessage(object sender, TeamCastChatMessage msg);
	}
}
