using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Flattiverse;
using YAFBCore.Messaging.Listeners;

namespace YAFBCore.Mapping.Units
{
	public class MissionTargetMapUnit : MapUnit, 
        IMissionTargetAvailableMessageListener,
        ITargetDedominationStartedMessageListener,
        ITargetDominationFinishedMessageListener,
        ITargetDominationStartedMessageListener,
        IPlayerUnitHitMissionTargetMessageListener
	{
		private MissionTarget missionTarget;
	
		public MissionTargetMapUnit(Map map, MissionTarget missionTarget, Vector movementOffset)
			: base(map, missionTarget, movementOffset)
		{
			this.missionTarget = missionTarget;

            DominationRadius = missionTarget.DominationRadius;
            Team = missionTarget.Team;
		}

        /// <summary>
        /// 
        /// </summary>
        public override int AgeMax => 50;

        /// <summary>
        /// 
        /// </summary>
        public override bool IsAging => true;

        /// <summary>
        /// 
        /// </summary>
        public override Mobility Mobility => Mobility.Mobile;

        /// <summary>
        /// 
        /// </summary>
        public float DominationRadius { get; }

        /// <summary>
        /// 
        /// </summary>
        public Team DominatingTeam { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public override bool HasListeners => true;

        /// <summary>
        /// 
        /// </summary>
        public bool IsHit { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyCollection<Vector> Hints => missionTarget.Hints;

        /// <summary>
        /// 
        /// </summary>
        public int SequenceNumber => missionTarget.SequenceNumber;

        /// <summary>
        /// 
        /// </summary>
        public Team Team { get; private set; }

        #region Event Listeners
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        public void OnMissionTargetAvailableMessage(object sender, MissionTargetAvailableMessage msg)
        {
            if (Name == msg.MissionTargetName)
                IsHit = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        public void OnPlayerUnitHitMissionTargetMessage(object sender, PlayerUnitHitMissionTargetMessage msg)
        {
            if (Name == msg.MissionTargetName 
                && Map.Universe.Connector.Player.Name == msg.SuccessfulPlayerUnitPlayer.Name)
                IsHit = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        public void OnTargetDedominationStartedMessage(object sender, TargetDedominationStartedMessage msg)
        {
            if (Name == msg.MissionTargetName)
                DominatingTeam = msg.DominatingTeam;

            Console.WriteLine("Dedomination Message");
        }

        public void OnTargetDominationStartedMessage(object sender, TargetDominationStartedMessage msg)
        {
            if (Name == msg.MissionTargetName)
                DominatingTeam = msg.DominatingTeam;

            Console.WriteLine("Domination started Message");
        }

        public void OnTargetDominationFinishedMessage(object sender, TargetDominationFinishedMessage msg)
        {
            if (Name == msg.MissionTargetName)
            {
                DominatingTeam = null;
                Team = msg.DominatingTeam;
            }

            Console.WriteLine("Domination finished Message");
        }
        #endregion
    }
}
