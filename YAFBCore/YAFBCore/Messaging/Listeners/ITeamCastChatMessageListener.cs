using System;
using Flattiverse;

namespace YAFBCore.Messaging.Listeners
{
	public interface ITeamCastChatMessageListener
	{
		void OnTeamCastChatMessage(object sender, TeamCastChatMessage msg);
	}
}
