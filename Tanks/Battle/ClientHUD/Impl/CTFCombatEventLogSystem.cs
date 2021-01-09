namespace Tanks.Battle.ClientHUD.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientHUD.API;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class CTFCombatEventLogSystem : ECSSystem
    {
        [OnEventFire]
        public void AddFlagNotCountedMessageLog(FlagNotCountedDeliveryEvent e, CTFBattleNode battle, [JoinAll] CombatLogNode combatLog)
        {
            combatLog.uiLog.UILog.AddMessage(combatLog.combatLogCTFMessages.DeliveryNotCounted);
        }

        [OnEventFire]
        public void AddMessageAutoReturnedFlag(FlagReturnEvent e, NotCarriedFlagNode flag, [JoinByTeam] TeamNode flagTeam, [JoinAll] SingleNode<SelfBattleUserComponent> selfUser, [JoinByTeam] Optional<TeamNode> selfTeam, [JoinAll] CombatLogNode combatEventLog)
        {
            CombatLogCTFMessagesComponent combatLogCTFMessages = combatEventLog.combatLogCTFMessages;
            string newValue = GetOwnFlag(selfTeam, flagTeam, combatLogCTFMessages);
            string messageText = combatLogCTFMessages.AutoReturned.Replace(CombatLogCTFMessagesComponent.OWN, newValue);
            combatEventLog.uiLog.UILog.AddMessage(messageText);
        }

        [OnEventFire]
        public void AddMessageLog(FlagEvent e, FlagNode flag, [JoinByTank] TankNode tank2Team, [JoinByTeam] TeamNode tankTeam, FlagNode flag2User, [JoinByTank] TankNode tank2User, [JoinByUser] UserNode user, FlagNode flag2Team, [JoinByTeam] TeamNode flagTeam, [JoinAll] SingleNode<SelfBattleUserComponent> selfUser, [JoinByTeam] Optional<TeamNode> selfTeam, [JoinAll] CombatLogNode combatLog)
        {
            CombatLogCTFMessagesComponent combatLogCTFMessages = combatLog.combatLogCTFMessages;
            string newValue = GetOwnFlag(selfTeam, flagTeam, combatLogCTFMessages);
            Color teamColor = CombatEventLogUtil.GetTeamColor(tankTeam.colorInBattle.TeamColor, combatLog.combatEventLog);
            string messageText = CombatEventLogUtil.ApplyPlaceholder(GetMessage(e, flag.Entity, combatLogCTFMessages).Replace(CombatLogCTFMessagesComponent.OWN, newValue), "{user}", user.userRank.Rank, user.userUid.Uid, teamColor);
            combatLog.uiLog.UILog.AddMessage(messageText);
        }

        [OnEventFire]
        public void CTFStartMessage(NodeAddedEvent e, BattleUserNode selfTank, [JoinByBattle] CTFBattleNode ctfBattle, [Context, JoinAll] CombatLogNode combatEventLog)
        {
            string battleStartMessage = combatEventLog.combatLogCTFMessages.BattleStartMessage;
            combatEventLog.uiLog.UILog.AddMessage(battleStartMessage);
        }

        [OnEventFire]
        public void FlagHomeStateComponent(NodeAddedEvent e, SingleNode<Tanks.Battle.ClientCore.Impl.FlagHomeStateComponent> n)
        {
        }

        private static string GetMessage(FlagEvent e, Entity flag, CombatLogCTFMessagesComponent logCtfMessages)
        {
            Type objA = e.GetType();
            if (ReferenceEquals(objA, typeof(FlagPickupEvent)))
            {
                return (!flag.HasComponent<Tanks.Battle.ClientCore.Impl.FlagHomeStateComponent>() ? logCtfMessages.PickedUp : logCtfMessages.Took);
            }
            if (ReferenceEquals(objA, typeof(FlagDropEvent)))
            {
                return (!((FlagDropEvent) e).IsUserAction ? logCtfMessages.Lost : logCtfMessages.Dropped);
            }
            if (ReferenceEquals(objA, typeof(FlagDeliveryEvent)))
            {
                return logCtfMessages.Delivered;
            }
            if (!ReferenceEquals(objA, typeof(FlagReturnEvent)))
            {
                throw new ArgumentException();
            }
            return logCtfMessages.Returned;
        }

        private static string GetOwnFlag(Optional<TeamNode> selfTeam, TeamNode flagTeam, CombatLogCTFMessagesComponent logCtfMessages) => 
            !selfTeam.IsPresent() ? ((flagTeam.colorInBattle.TeamColor != TeamColor.BLUE) ? logCtfMessages.RedFlag : logCtfMessages.BlueFlag) : ((flagTeam.teamGroup.Key != selfTeam.Get().teamGroup.Key) ? logCtfMessages.EnemyFlag : logCtfMessages.OurFlag);

        public class BattleUserNode : Node
        {
            public SelfComponent self;
            public BattleUserComponent battleUser;
            public UserInBattleAsTankComponent userInBattleAsTank;
            public UserGroupComponent userGroup;
            public BattleGroupComponent battleGroup;
        }

        public class CombatLogNode : Node
        {
            public CombatLogCTFMessagesComponent combatLogCTFMessages;
            public CombatEventLogComponent combatEventLog;
            public ActiveCombatLogComponent activeCombatLog;
            public UILogComponent uiLog;
        }

        public class CTFBattleNode : Node
        {
            public BattleComponent battle;
            public CTFComponent ctf;
        }

        public class FlagNode : Node
        {
            public FlagComponent flag;
            public TeamGroupComponent teamGroup;
        }

        [Not(typeof(TankGroupComponent))]
        public class NotCarriedFlagNode : CTFCombatEventLogSystem.FlagNode
        {
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public TeamGroupComponent teamGroup;
            public TankGroupComponent tankGroup;
        }

        public class TeamNode : Node
        {
            public TeamComponent team;
            public TeamGroupComponent teamGroup;
            public ColorInBattleComponent colorInBattle;
        }

        public class UserNode : Node
        {
            public UserGroupComponent userGroup;
            public UserComponent user;
            public UserUidComponent userUid;
            public UserRankComponent userRank;
        }
    }
}

