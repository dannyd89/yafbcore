using Flattiverse;
using YAFBCore.Flattiverse.Messaging.Listeners;
using YAFBCore.Flattiverse.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YAFBCore.Flattiverse.Messaging
{
    #region Flattiverse messages eventhandlers
    public delegate void MessageEventHandler(object sender, FlattiverseMessage msg);
    public delegate void BinaryChatMessageEventHandler(object sender, BinaryChatMessage msg);
    public delegate void BroadCastChatMessageEventHandler(object sender, BroadCastChatMessage msg);
    public delegate void ChatMessageEventHandler(object sender, ChatMessage msg);
    public delegate void GameMessageEventHandler(object sender, GameMessage msg);
    public delegate void GateSwitchedMessageEventHandler(object sender, GateSwitchedMessage msg);
    public delegate void MissionTargetAvailableMessageEventHandler(object sender, MissionTargetAvailableMessage msg);
    public delegate void MOTDMessageEventHandler(object sender, MOTDMessage msg);
    public delegate void PlayerDroppedFromUniverseGroupMessageEventHandler(object sender, PlayerDroppedFromUniverseGroupMessage msg);
    public delegate void PlayerJoinedUniverseGroupMessageEventHandler(object sender, PlayerJoinedUniverseGroupMessage msg);
    public delegate void PlayerKickedFromUniverseGroupMessageEventHandler(object sender, PlayerKickedFromUniverseGroupMessage msg);
    public delegate void PlayerPartedUniverseGroupMessageEventHandler(object sender, PlayerPartedUniverseGroupMessage msg);
    public delegate void PlayerUnitBuildCanceledMessageEventHandler(object sender, PlayerUnitBuildCanceledMessage msg);
    public delegate void PlayerUnitBuildFinishedMessageEventHandler(object sender, PlayerUnitBuildFinishedMessage msg);
    public delegate void PlayerUnitBuildMessageEventHandler(object sender, PlayerUnitBuildMessage msg);
    public delegate void PlayerUnitBuildStartMessageEventHandler(object sender, PlayerUnitBuildStartMessage msg);
    public delegate void PlayerUnitCollidedWithPlayerUnitMessageEventHandler(object sender, PlayerUnitCollidedWithPlayerUnitMessage msg);
    public delegate void PlayerUnitCollidedWithUnitMessageEventHandler(object sender, PlayerUnitCollidedWithUnitMessage msg);
    public delegate void PlayerUnitCommittedSuicideMessageEventHandler(object sender, PlayerUnitCommittedSuicideMessage msg);
    public delegate void PlayerUnitContinuedMessageEventHandler(object sender, PlayerUnitContinuedMessage msg);
    public delegate void PlayerUnitDeceasedByBadHullRefreshingPowerUpMessageEventHandler(object sender, PlayerUnitDeceasedByBadHullRefreshingPowerUpMessage msg);
    public delegate void PlayerUnitDeceasedMessageEventHandler(object sender, PlayerUnitDeceasedMessage msg);
    public delegate void PlayerUnitHitEnemyTargetMessageEventHandler(object sender, PlayerUnitHitEnemyTargetMessage msg);
    public delegate void PlayerUnitHitMissionTargetMessageEventHandler(object sender, PlayerUnitHitMissionTargetMessage msg);
    public delegate void PlayerUnitHitOwnTargetMessageEventHandler(object sender, PlayerUnitHitOwnTargetMessage msg);
    public delegate void PlayerUnitJumpedMessageEventHandler(object sender, PlayerUnitJumpedMessage msg);
    public delegate void PlayerUnitLoggedOffMessageEventHandler(object sender, PlayerUnitLoggedOffMessage msg);
    public delegate void PlayerUnitShotByPlayerUnitMessageEventHandler(object sender, PlayerUnitShotByPlayerUnitMessage msg);
    public delegate void PlayerUnitShotByUnitMessageEventHandler(object sender, PlayerUnitShotByUnitMessage msg);
    public delegate void SystemMessageEventHandler(object sender, SystemMessage msg);
    public delegate void TargetDedominationStartedMessageEventHandler(object sender, TargetDedominationStartedMessage msg);
    public delegate void TargetDominationFinishedMessageEventHandler(object sender, TargetDominationFinishedMessage msg);
    public delegate void TargetDominationScoredMessageEventHandler(object sender, TargetDominationScoredMessage msg);
    public delegate void TargetDominationStartedMessageEventHandler(object sender, TargetDominationStartedMessage msg);
    public delegate void TeamCastChatMessageEventHandler(object sender, TeamCastChatMessage msg);

    public delegate void UniCastChatMessageEventHandler(object sender, UniCastChatMessage msg);

    public delegate void UniverseGroupResetMessageEventHandler(object sender, UniverseGroupResetMessage msg);
    public delegate void UniverseGroupResetPendingMessageEventHandler(object sender, UniverseGroupResetPendingMessage msg);
    #endregion

    public class MessageManager
    {
        internal readonly Connection Connection;

        #region Events
        
        public event BinaryChatMessageEventHandler BinaryChatMessageReceived;
        public event BroadCastChatMessageEventHandler BroadCastChatMessageReceived;
        public event ChatMessageEventHandler ChatMessageReceived;
        public event GameMessageEventHandler GameMessageReceived;
        public event GateSwitchedMessageEventHandler GateSwitchedMessageReceived;
        public event MissionTargetAvailableMessageEventHandler MissionTargetAvailableMessageReceived;
        public event MOTDMessageEventHandler MOTDMessageReceived;
        public event PlayerDroppedFromUniverseGroupMessageEventHandler PlayerDroppedFromUniverseGroupMessageReceived;
        public event PlayerJoinedUniverseGroupMessageEventHandler PlayerJoinedUniverseGroupMessageReceived;
        public event PlayerKickedFromUniverseGroupMessageEventHandler PlayerKickedFromUniverseGroupMessageReceived;
        public event PlayerPartedUniverseGroupMessageEventHandler PlayerPartedUniverseGroupMessageReceived;
        public event PlayerUnitBuildCanceledMessageEventHandler PlayerUnitBuildCanceledMessageReceived;
        public event PlayerUnitBuildFinishedMessageEventHandler PlayerUnitBuildFinishedMessageReceived;
        public event PlayerUnitBuildMessageEventHandler PlayerUnitBuildMessageReceived;
        public event PlayerUnitBuildStartMessageEventHandler PlayerUnitBuildStartMessageReceived;
        public event PlayerUnitCollidedWithPlayerUnitMessageEventHandler PlayerUnitCollidedWithPlayerUnitMessageReceived;
        public event PlayerUnitCollidedWithUnitMessageEventHandler PlayerUnitCollidedWithUnitMessageReceived;
        public event PlayerUnitCommittedSuicideMessageEventHandler PlayerUnitCommittedSuicideMessageReceived;
        public event PlayerUnitContinuedMessageEventHandler PlayerUnitContinuedMessageReceived;
        public event PlayerUnitDeceasedByBadHullRefreshingPowerUpMessageEventHandler PlayerUnitDeceasedByBadHullRefreshingPowerUpMessageReceived;
        public event PlayerUnitDeceasedMessageEventHandler PlayerUnitDeceasedMessageReceived;
        public event PlayerUnitHitEnemyTargetMessageEventHandler PlayerUnitHitEnemyTargetMessageReceived;
        public event PlayerUnitHitMissionTargetMessageEventHandler PlayerUnitHitMissionTargetMessageReceived;
        public event PlayerUnitHitOwnTargetMessageEventHandler PlayerUnitHitOwnTargetMessageReceived;
        public event PlayerUnitJumpedMessageEventHandler PlayerUnitJumpedMessageReceived;
        public event PlayerUnitLoggedOffMessageEventHandler PlayerUnitLoggedOffMessageReceived;
        public event PlayerUnitShotByPlayerUnitMessageEventHandler PlayerUnitShotByPlayerUnitMessageReceived;
        public event PlayerUnitShotByUnitMessageEventHandler PlayerUnitShotByUnitMessageReceived;
        public event SystemMessageEventHandler SystemMessageReceived;
        public event TargetDedominationStartedMessageEventHandler TargetDeDominationStartedMessageReceived;
        public event TargetDominationFinishedMessageEventHandler TargetDominationFinishedMessageReceived;
        public event TargetDominationScoredMessageEventHandler TargetDominationScoredMessageReceived;
        public event TargetDominationStartedMessageEventHandler TargetDominationStartedMessageReceived;
        public event TeamCastChatMessageEventHandler TeamCastChatMessageReceived;
        public event UniCastChatMessageEventHandler UniCastChatMessageReceived;
        public event UniverseGroupResetMessageEventHandler UniverseGroupResetMessageReceived;
        public event UniverseGroupResetPendingMessageEventHandler UniverseGroupResetPendingMessageReceived;

        #endregion

        /// <summary>
        /// Creates a message manager and starts the recieving task to handle incoming messages
        /// </summary>
        /// <param name="connection"></param>
        internal MessageManager(Connection connection)
        {
            Connection = connection;
        }
        
        /// <summary>
        /// Adds a listener for specific messages sent by the flattiverse server
        /// </summary>
        /// <param name="listener"></param>
        internal void AddListener(object listener)
        {
            IBinaryChatMessageListener binaryChatMessageListener = listener as IBinaryChatMessageListener;
            if (binaryChatMessageListener != null)
                BinaryChatMessageReceived += binaryChatMessageListener.OnBinaryChatMessage;

            IBroadCastChatMessageListener broadCastChatMessageListener = listener as IBroadCastChatMessageListener;
            if (broadCastChatMessageListener != null)
                BroadCastChatMessageReceived += broadCastChatMessageListener.OnBroadCastChatMessage;

            IChatMessageListener chatMessageListener = listener as IChatMessageListener;
            if (chatMessageListener != null)
                ChatMessageReceived += chatMessageListener.OnChatMessage;

            IGateSwitchedMessageListener gateSwitchedMessageListener = listener as IGateSwitchedMessageListener;
            if (gateSwitchedMessageListener != null)
                GateSwitchedMessageReceived += gateSwitchedMessageListener.OnGateSwitchedMessage;

            IPlayerKickedFromUniverseGroupMessageListener playerKickedFromUniverseGroupMessageListener = listener as IPlayerKickedFromUniverseGroupMessageListener;
            if (playerKickedFromUniverseGroupMessageListener != null)
                PlayerKickedFromUniverseGroupMessageReceived += playerKickedFromUniverseGroupMessageListener.OnPlayerKickedFromUniverseGroupMessage;

            IPlayerUnitBuildCanceledMessageListener playerUnitBuildCanceledMessageListener = listener as IPlayerUnitBuildCanceledMessageListener;
            if (playerUnitBuildCanceledMessageListener != null)
                PlayerUnitBuildCanceledMessageReceived += playerUnitBuildCanceledMessageListener.OnPlayerUnitBuildCanceledMessage;

            IPlayerUnitBuildFinishedMessageListener playerUnitBuildFinishedMessageListener = listener as IPlayerUnitBuildFinishedMessageListener;
            if (playerUnitBuildFinishedMessageListener != null)
                PlayerUnitBuildFinishedMessageReceived += playerUnitBuildFinishedMessageListener.OnPlayerUnitBuildFinishedMessage;

            IPlayerUnitBuildMessageListener playerUnitBuildMessageListener = listener as IPlayerUnitBuildMessageListener;
            if (playerUnitBuildMessageListener != null)
                PlayerUnitBuildMessageReceived += playerUnitBuildMessageListener.OnPlayerUnitBuildMessage;

            IGameMessageListener gameMessageListener = listener as IGameMessageListener;
            if (gameMessageListener != null)
                GameMessageReceived += gameMessageListener.OnGameMessage;

            IMissionTargetAvailableMessageListener missionTargetAvailableMessageListener = listener as IMissionTargetAvailableMessageListener;
            if (missionTargetAvailableMessageListener != null)
                MissionTargetAvailableMessageReceived += missionTargetAvailableMessageListener.OnMissionTargetAvailableMessage;

            IMOTDMessageListener mOTDMessageListener = listener as IMOTDMessageListener;
            if (mOTDMessageListener != null)
                MOTDMessageReceived += mOTDMessageListener.OnMOTDMessage;

            IPlayerDroppedFromUniverseGroupMessageListener playerDroppedFromUniverseGroupMessageListener = listener as IPlayerDroppedFromUniverseGroupMessageListener;
            if (playerDroppedFromUniverseGroupMessageListener != null)
                PlayerDroppedFromUniverseGroupMessageReceived += playerDroppedFromUniverseGroupMessageListener.OnPlayerDroppedFromUniverseGroupMessage;

            IPlayerJoinedUniverseGroupMessageListener playerJoinedUniverseGroupMessageListener = listener as IPlayerJoinedUniverseGroupMessageListener;
            if (playerJoinedUniverseGroupMessageListener != null)
                PlayerJoinedUniverseGroupMessageReceived += playerJoinedUniverseGroupMessageListener.OnPlayerJoinedUniverseGroupMessage;

            IPlayerPartedUniverseGroupMessageListener playerPartedUniverseGroupMessageListener = listener as IPlayerPartedUniverseGroupMessageListener;
            if (playerPartedUniverseGroupMessageListener != null)
                PlayerPartedUniverseGroupMessageReceived += playerPartedUniverseGroupMessageListener.OnPlayerPartedUniverseGroupMessage;

            IPlayerUnitBuildStartMessageListener playerUnitBuildStartMessageListener = listener as IPlayerUnitBuildStartMessageListener;
            if (playerUnitBuildStartMessageListener != null)
                PlayerUnitBuildStartMessageReceived += playerUnitBuildStartMessageListener.OnPlayerUnitBuildStartMessage;

            IPlayerUnitCollidedWithPlayerUnitMessageListener playerUnitCollidedWithPlayerUnitMessageListener = listener as IPlayerUnitCollidedWithPlayerUnitMessageListener;
            if (playerUnitCollidedWithPlayerUnitMessageListener != null)
                PlayerUnitCollidedWithPlayerUnitMessageReceived += playerUnitCollidedWithPlayerUnitMessageListener.OnPlayerUnitCollidedWithPlayerUnitMessage;

            IPlayerUnitCollidedWithUnitMessageListener playerUnitCollidedWithUnitMessageListener = listener as IPlayerUnitCollidedWithUnitMessageListener;
            if (playerUnitCollidedWithUnitMessageListener != null)
                PlayerUnitCollidedWithUnitMessageReceived += playerUnitCollidedWithUnitMessageListener.OnPlayerUnitCollidedWithUnitMessage;

            IPlayerUnitCommittedSuicideMessageListener playerUnitCommittedSuicideMessageListener = listener as IPlayerUnitCommittedSuicideMessageListener;
            if (playerUnitCommittedSuicideMessageListener != null)
                PlayerUnitCommittedSuicideMessageReceived += playerUnitCommittedSuicideMessageListener.OnPlayerUnitCommittedSuicideMessage;

            IPlayerUnitContinuedMessageListener playerUnitContinuedMessageListener = listener as IPlayerUnitContinuedMessageListener;
            if (playerUnitContinuedMessageListener != null)
                PlayerUnitContinuedMessageReceived += playerUnitContinuedMessageListener.OnPlayerUnitContinuedMessage;

            IPlayerUnitDeceasedByBadHullRefreshingPowerUpMessageListener playerUnitDeceasedByBadHullRefreshingPowerUpMessageListener = listener as IPlayerUnitDeceasedByBadHullRefreshingPowerUpMessageListener;
            if (playerUnitDeceasedByBadHullRefreshingPowerUpMessageListener != null)
                PlayerUnitDeceasedByBadHullRefreshingPowerUpMessageReceived += playerUnitDeceasedByBadHullRefreshingPowerUpMessageListener.OnPlayerUnitDeceasedByBadHullRefreshingPowerUpMessage;

            IPlayerUnitDeceasedMessageListener playerUnitDeceasedMessageListener = listener as IPlayerUnitDeceasedMessageListener;
            if (playerUnitDeceasedMessageListener != null)
                PlayerUnitDeceasedMessageReceived += playerUnitDeceasedMessageListener.OnPlayerUnitDeceasedMessage;

            IPlayerUnitHitEnemyTargetMessageListener playerUnitHitEnemyTargetMessageListener = listener as IPlayerUnitHitEnemyTargetMessageListener;
            if (playerUnitHitEnemyTargetMessageListener != null)
                PlayerUnitHitEnemyTargetMessageReceived += playerUnitHitEnemyTargetMessageListener.OnPlayerUnitHitEnemyTargetMessage;

            IPlayerUnitHitMissionTargetMessageListener playerUnitHitMissionTargetMessageListener = listener as IPlayerUnitHitMissionTargetMessageListener;
            if (playerUnitHitMissionTargetMessageListener != null)
                PlayerUnitHitMissionTargetMessageReceived += playerUnitHitMissionTargetMessageListener.OnPlayerUnitHitMissionTargetMessage;

            IPlayerUnitHitOwnTargetMessageListener playerUnitHitOwnTargetMessageListener = listener as IPlayerUnitHitOwnTargetMessageListener;
            if (playerUnitHitOwnTargetMessageListener != null)
                PlayerUnitHitOwnTargetMessageReceived += playerUnitHitOwnTargetMessageListener.OnPlayerUnitHitOwnTargetMessage;

            IPlayerUnitJumpedMessageListener playerUnitJumpedMessageListener = listener as IPlayerUnitJumpedMessageListener;
            if (playerUnitJumpedMessageListener != null)
                PlayerUnitJumpedMessageReceived += playerUnitJumpedMessageListener.OnPlayerUnitJumpedMessage;

            IPlayerUnitLoggedOffMessageListener playerUnitLoggedOffMessageListener = listener as IPlayerUnitLoggedOffMessageListener;
            if (playerUnitLoggedOffMessageListener != null)
                PlayerUnitLoggedOffMessageReceived += playerUnitLoggedOffMessageListener.OnPlayerUnitLoggedOffMessage;

            IPlayerUnitShotByPlayerUnitMessageListener playerUnitShotByPlayerUnitMessageListener = listener as IPlayerUnitShotByPlayerUnitMessageListener;
            if (playerUnitShotByPlayerUnitMessageListener != null)
                PlayerUnitShotByPlayerUnitMessageReceived += playerUnitShotByPlayerUnitMessageListener.OnPlayerUnitShotByPlayerUnitMessage;

            IPlayerUnitShotByUnitMessageListener playerUnitShotByUnitMessageListener = listener as IPlayerUnitShotByUnitMessageListener;
            if (playerUnitShotByUnitMessageListener != null)
                PlayerUnitShotByUnitMessageReceived += playerUnitShotByUnitMessageListener.OnPlayerUnitShotByUnitMessage;

            ISystemMessageListener systemMessageListener = listener as ISystemMessageListener;
            if (systemMessageListener != null)
                SystemMessageReceived += systemMessageListener.OnSystemMessage;

            ITargetDedominationStartedMessageListener targetDedominationStartedMessageListener = listener as ITargetDedominationStartedMessageListener;
            if (targetDedominationStartedMessageListener != null)
                TargetDeDominationStartedMessageReceived += targetDedominationStartedMessageListener.OnTargetDedominationStartedMessage;

            ITargetDominationFinishedMessageListener targetDominationFinishedMessageListener = listener as ITargetDominationFinishedMessageListener;
            if (targetDominationFinishedMessageListener != null)
                TargetDominationFinishedMessageReceived += targetDominationFinishedMessageListener.OnTargetDominationFinishedMessage;

            ITargetDominationScoredMessageListener targetDominationScoredMessageListener = listener as ITargetDominationScoredMessageListener;
            if (targetDominationScoredMessageListener != null)
                TargetDominationScoredMessageReceived += targetDominationScoredMessageListener.OnTargetDominationScoredMessage;

            ITargetDominationStartedMessageListener targetDominationStartedMessageListener = listener as ITargetDominationStartedMessageListener;
            if (targetDominationStartedMessageListener != null)
                TargetDominationStartedMessageReceived += targetDominationStartedMessageListener.OnTargetDominationStartedMessage;

            ITeamCastChatMessageListener teamCastChatMessageListener = listener as ITeamCastChatMessageListener;
            if (teamCastChatMessageListener != null)
                TeamCastChatMessageReceived += teamCastChatMessageListener.OnTeamCastChatMessage;

            IUniCastChatMessageListener uniCastChatMessageListener = listener as IUniCastChatMessageListener;
            if (uniCastChatMessageListener != null)
                UniCastChatMessageReceived += uniCastChatMessageListener.OnUniCastChatMessage;

            IUniverseGroupResetMessageListener universeGroupResetMessageListener = listener as IUniverseGroupResetMessageListener;
            if (universeGroupResetMessageListener != null)
                UniverseGroupResetMessageReceived += universeGroupResetMessageListener.OnUniverseGroupResetMessage;

            IUniverseGroupResetPendingMessageListener universeGroupResetPendingMessageListener = listener as IUniverseGroupResetPendingMessageListener;
            if (universeGroupResetPendingMessageListener != null)
                UniverseGroupResetPendingMessageReceived += universeGroupResetPendingMessageListener.OnUniverseGroupResetPendingMessage;

        }

        /// <summary>
        /// Handles incoming messages
        /// </summary>
        internal void ReadMessages()
        {
            FlattiverseMessage message;

            while (Connection.Connector.NextMessage(out message))
            {
                if (message is BinaryChatMessage)
                    RaiseOnMessage((BinaryChatMessage)message);
                else if (message is BroadCastChatMessage)
                    RaiseOnMessage((BroadCastChatMessage)message);
                else if (message is ChatMessage)
                    RaiseOnMessage((ChatMessage)message);
                else if (message is GameMessage)
                    RaiseOnMessage((GameMessage)message);
                else if (message is GateSwitchedMessage)
                    RaiseOnMessage((GateSwitchedMessage)message);
                else if (message is MissionTargetAvailableMessage)
                    RaiseOnMessage((MissionTargetAvailableMessage)message);
                else if (message is MOTDMessage)
                    RaiseOnMessage((MOTDMessage)message);
                else if (message is PlayerDroppedFromUniverseGroupMessage)
                    RaiseOnMessage((PlayerDroppedFromUniverseGroupMessage)message);
                else if (message is PlayerJoinedUniverseGroupMessage)
                    RaiseOnMessage((PlayerJoinedUniverseGroupMessage)message);
                else if (message is PlayerKickedFromUniverseGroupMessage)
                    RaiseOnMessage((PlayerKickedFromUniverseGroupMessage)message);
                else if (message is PlayerPartedUniverseGroupMessage)
                    RaiseOnMessage((PlayerPartedUniverseGroupMessage)message);
                else if (message is PlayerUnitBuildCanceledMessage)
                    RaiseOnMessage((PlayerUnitBuildCanceledMessage)message);
                else if (message is PlayerUnitBuildFinishedMessage)
                    RaiseOnMessage((PlayerUnitBuildFinishedMessage)message);
                else if (message is PlayerUnitBuildMessage)
                    RaiseOnMessage((PlayerUnitBuildMessage)message);
                else if (message is PlayerUnitBuildStartMessage)
                    RaiseOnMessage((PlayerUnitBuildStartMessage)message);
                else if (message is PlayerUnitCollidedWithPlayerUnitMessage)
                    RaiseOnMessage((PlayerUnitCollidedWithPlayerUnitMessage)message);
                else if (message is PlayerUnitCollidedWithUnitMessage)
                    RaiseOnMessage((PlayerUnitCollidedWithUnitMessage)message);
                else if (message is PlayerUnitCommittedSuicideMessage)
                    RaiseOnMessage((PlayerUnitCommittedSuicideMessage)message);
                else if (message is PlayerUnitContinuedMessage)
                    RaiseOnMessage((PlayerUnitContinuedMessage)message);
                else if (message is PlayerUnitDeceasedByBadHullRefreshingPowerUpMessage)
                    RaiseOnMessage((PlayerUnitDeceasedByBadHullRefreshingPowerUpMessage)message);
                else if (message is PlayerUnitDeceasedMessage)
                    RaiseOnMessage((PlayerUnitDeceasedMessage)message);
                else if (message is PlayerUnitHitEnemyTargetMessage)
                    RaiseOnMessage((PlayerUnitHitEnemyTargetMessage)message);
                else if (message is PlayerUnitHitMissionTargetMessage)
                    RaiseOnMessage((PlayerUnitHitMissionTargetMessage)message);
                else if (message is PlayerUnitHitOwnTargetMessage)
                    RaiseOnMessage((PlayerUnitHitOwnTargetMessage)message);
                else if (message is PlayerUnitJumpedMessage)
                    RaiseOnMessage((PlayerUnitJumpedMessage)message);
                else if (message is PlayerUnitLoggedOffMessage)
                    RaiseOnMessage((PlayerUnitLoggedOffMessage)message);
                else if (message is PlayerUnitShotByPlayerUnitMessage)
                    RaiseOnMessage((PlayerUnitShotByPlayerUnitMessage)message);
                else if (message is PlayerUnitShotByUnitMessage)
                    RaiseOnMessage((PlayerUnitShotByUnitMessage)message);
                else if (message is SystemMessage)
                    RaiseOnMessage((SystemMessage)message);
                else if (message is TargetDedominationStartedMessage)
                    RaiseOnMessage((TargetDedominationStartedMessage)message);
                else if (message is TargetDominationFinishedMessage)
                    RaiseOnMessage((TargetDominationFinishedMessage)message);
                else if (message is TargetDominationScoredMessage)
                    RaiseOnMessage((TargetDominationScoredMessage)message);
                else if (message is TargetDominationStartedMessage)
                    RaiseOnMessage((TargetDominationStartedMessage)message);
                else if (message is TeamCastChatMessage)
                    RaiseOnMessage((TeamCastChatMessage)message);
                else if (message is UniCastChatMessage)
                    RaiseOnMessage((UniCastChatMessage)message);
                else if (message is UniverseGroupResetMessage)
                    RaiseOnMessage((UniverseGroupResetMessage)message);
                else if (message is UniverseGroupResetPendingMessage)
                    RaiseOnMessage((UniverseGroupResetPendingMessage)message);
                else
                    RaiseOnMessage(message.GetType().Name);
            }
        }

        #region RaiseOnMessage Functions
        private void RaiseOnMessage(string typeName)
        {
            System.Diagnostics.Debug.WriteLine($"Unknown message with type {typeName} recieved");
        }

        private void RaiseOnMessage(BinaryChatMessage binaryChatMessage)
        {
            BinaryChatMessageReceived?.Invoke(this, binaryChatMessage);
        }

        private void RaiseOnMessage(BroadCastChatMessage broadCastChatMessage)
        {
            BroadCastChatMessageReceived?.Invoke(this, broadCastChatMessage);
        }

        private void RaiseOnMessage(ChatMessage chatMessage)
        {
            ChatMessageReceived?.Invoke(this, chatMessage);
        }

        private void RaiseOnMessage(GameMessage gameMessage)
        {
            GameMessageReceived?.Invoke(this, gameMessage);
        }

        private void RaiseOnMessage(GateSwitchedMessage gateSwitchedMessage)
        {
            GateSwitchedMessageReceived?.Invoke(this, gateSwitchedMessage);
        }

        private void RaiseOnMessage(MissionTargetAvailableMessage missionTargetAvailableMessage)
        {
            MissionTargetAvailableMessageReceived?.Invoke(this, missionTargetAvailableMessage);
        }

        private void RaiseOnMessage(MOTDMessage motdMessage)
        {
            MOTDMessageReceived?.Invoke(this, motdMessage);
        }

        private void RaiseOnMessage(PlayerDroppedFromUniverseGroupMessage playerDroppedFromUniverseGroupMessage)
        {
            PlayerDroppedFromUniverseGroupMessageReceived?.Invoke(this, playerDroppedFromUniverseGroupMessage);
        }

        private void RaiseOnMessage(PlayerJoinedUniverseGroupMessage playerJoinedUniverseGroupMessage)
        {
            PlayerJoinedUniverseGroupMessageReceived?.Invoke(this, playerJoinedUniverseGroupMessage);
        }

        private void RaiseOnMessage(PlayerKickedFromUniverseGroupMessage playerKickedFromUniverseGroupMessage)
        {
            PlayerKickedFromUniverseGroupMessageReceived?.Invoke(this, playerKickedFromUniverseGroupMessage);
        }

        private void RaiseOnMessage(PlayerPartedUniverseGroupMessage playerPartedUniverseGroupMessage)
        {
            PlayerPartedUniverseGroupMessageReceived?.Invoke(this, playerPartedUniverseGroupMessage);
        }

        private void RaiseOnMessage(PlayerUnitBuildCanceledMessage playerUnitBuildCanceledMessage)
        {
            PlayerUnitBuildCanceledMessageReceived?.Invoke(this, playerUnitBuildCanceledMessage);
        }

        private void RaiseOnMessage(PlayerUnitBuildFinishedMessage playerUnitBuildFinishedMessage)
        {
            PlayerUnitBuildFinishedMessageReceived?.Invoke(this, playerUnitBuildFinishedMessage);
        }

        private void RaiseOnMessage(PlayerUnitBuildMessage playerUnitBuildMessage)
        {
            PlayerUnitBuildMessageReceived?.Invoke(this, playerUnitBuildMessage);
        }

        private void RaiseOnMessage(PlayerUnitBuildStartMessage playerUnitBuildStartMessage)
        {
            PlayerUnitBuildStartMessageReceived?.Invoke(this, playerUnitBuildStartMessage);
        }

        private void RaiseOnMessage(PlayerUnitCollidedWithPlayerUnitMessage playerUnitCollidedWithPlayerUnitMessage)
        {
            PlayerUnitCollidedWithPlayerUnitMessageReceived?.Invoke(this, playerUnitCollidedWithPlayerUnitMessage);
        }

        private void RaiseOnMessage(PlayerUnitCollidedWithUnitMessage playerUnitCollidedWithUnitMessage)
        {
            PlayerUnitCollidedWithUnitMessageReceived?.Invoke(this, playerUnitCollidedWithUnitMessage);
        }

        private void RaiseOnMessage(PlayerUnitCommittedSuicideMessage playerUnitCommittedSuicideMessage)
        {
            PlayerUnitCommittedSuicideMessageReceived?.Invoke(this, playerUnitCommittedSuicideMessage);
        }

        private void RaiseOnMessage(PlayerUnitContinuedMessage playerUnitContinuedMessage)
        {
            PlayerUnitContinuedMessageReceived?.Invoke(this, playerUnitContinuedMessage);
        }

        private void RaiseOnMessage(PlayerUnitDeceasedByBadHullRefreshingPowerUpMessage playerUnitDeceasedByBadHullRefreshingPowerUpMessage)
        {
            PlayerUnitDeceasedByBadHullRefreshingPowerUpMessageReceived?.Invoke(this, playerUnitDeceasedByBadHullRefreshingPowerUpMessage);
        }

        private void RaiseOnMessage(PlayerUnitDeceasedMessage playerUnitDeceasedMessage)
        {
            PlayerUnitDeceasedMessageReceived?.Invoke(this, playerUnitDeceasedMessage);
        }

        private void RaiseOnMessage(PlayerUnitHitEnemyTargetMessage playerUnitHitEnemyTargetMessage)
        {
            PlayerUnitHitEnemyTargetMessageReceived?.Invoke(this, playerUnitHitEnemyTargetMessage);
        }

        private void RaiseOnMessage(PlayerUnitHitMissionTargetMessage playerUnitHitMissionTargetMessage)
        {
            PlayerUnitHitMissionTargetMessageReceived?.Invoke(this, playerUnitHitMissionTargetMessage);
        }

        private void RaiseOnMessage(PlayerUnitHitOwnTargetMessage playerUnitHitOwnTargetMessage)
        {
            PlayerUnitHitOwnTargetMessageReceived?.Invoke(this, playerUnitHitOwnTargetMessage);
        }

        private void RaiseOnMessage(PlayerUnitJumpedMessage playerUnitJumpedMessage)
        {
            PlayerUnitJumpedMessageReceived?.Invoke(this, playerUnitJumpedMessage);
        }

        private void RaiseOnMessage(PlayerUnitLoggedOffMessage playerUnitLoggedOffMessage)
        {
            PlayerUnitLoggedOffMessageReceived?.Invoke(this, playerUnitLoggedOffMessage);
        }

        private void RaiseOnMessage(PlayerUnitShotByPlayerUnitMessage playerUnitShotByPlayerUnitMessage)
        {
            PlayerUnitShotByPlayerUnitMessageReceived?.Invoke(this, playerUnitShotByPlayerUnitMessage);
        }

        private void RaiseOnMessage(PlayerUnitShotByUnitMessage playerUnitShotByUnitMessage)
        {
            PlayerUnitShotByUnitMessageReceived?.Invoke(this, playerUnitShotByUnitMessage);
        }

        private void RaiseOnMessage(SystemMessage systemMessage)
        {
            SystemMessageReceived?.Invoke(this, systemMessage);
        }

        private void RaiseOnMessage(TargetDedominationStartedMessage targetDedominationStartedMessage)
        {
            TargetDeDominationStartedMessageReceived?.Invoke(this, targetDedominationStartedMessage);
        }

        private void RaiseOnMessage(TargetDominationFinishedMessage targetDominationFinishedMessage)
        {
            TargetDominationFinishedMessageReceived?.Invoke(this, targetDominationFinishedMessage);
        }

        private void RaiseOnMessage(TargetDominationScoredMessage targetDominationScoredMessage)
        {
            TargetDominationScoredMessageReceived?.Invoke(this, targetDominationScoredMessage);
        }

        private void RaiseOnMessage(TargetDominationStartedMessage targetDominationStartedMessage)
        {
            TargetDominationStartedMessageReceived?.Invoke(this, targetDominationStartedMessage);
        }

        private void RaiseOnMessage(TeamCastChatMessage teamCastChatMessage)
        {
            TeamCastChatMessageReceived?.Invoke(this, teamCastChatMessage);
        }

        private void RaiseOnMessage(UniCastChatMessage uniCastChatMessage)
        {
            UniCastChatMessageReceived?.Invoke(this, uniCastChatMessage);
        }

        private void RaiseOnMessage(UniverseGroupResetMessage universeGroupResetMessage)
        {
            UniverseGroupResetMessageReceived?.Invoke(this, universeGroupResetMessage);
        }

        private void RaiseOnMessage(UniverseGroupResetPendingMessage universeGroupResetPendingMessage)
        {
            UniverseGroupResetPendingMessageReceived?.Invoke(this, universeGroupResetPendingMessage);
        }
        #endregion
    }
}
