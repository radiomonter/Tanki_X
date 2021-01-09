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

    public class CombatEventLogSystem : ECSSystem
    {
        [OnEventFire]
        public void ActivateCombatLog(NodeAddedEvent e, SelfBattleUserNode selfBattleUser, InactiveCombatLogNode combatLog)
        {
            combatLog.Entity.AddComponent<ActiveCombatLogComponent>();
        }

        [OnEventFire]
        public void AddUILogComponent(NodeAddedEvent evt, SingleNode<CombatEventLogComponent> combatEventLog)
        {
            combatEventLog.Entity.AddComponent(new UILogComponent(CombatEventLogUtil.GetUILog(combatEventLog.component)));
        }

        private void AddUserAddedMessage(NotSelfUserNode userNode, TeamColor userTeamColor, CombatEventLogNode combatEventLogNode)
        {
            Color teamColor = CombatEventLogUtil.GetTeamColor(userTeamColor, combatEventLogNode.combatEventLog);
            string messageText = CombatEventLogUtil.ApplyPlaceholder(combatEventLogNode.combatLogCommonMessages.UserJoinBattleMessage, "{user}", userNode.userRank.Rank, userNode.userUid.Uid, teamColor);
            combatEventLogNode.uiLog.UILog.AddMessage(messageText);
        }

        [OnEventFire]
        public void ClearCombatLogOnEnter(NodeAddedEvent e, CombatEventLogNode combatEventLog)
        {
            combatEventLog.uiLog.UILog.Clear();
        }

        [OnEventFire]
        public void ClearCombatLogOnExit(NodeRemoveEvent e, CombatEventLogNode combatEventLog)
        {
            combatEventLog.uiLog.UILog.Clear();
        }

        [OnEventFire]
        public void DeactivateCombatLogOnExit(NodeRemoveEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser, [JoinAll] CombatEventLogNode combatEventLog)
        {
            combatEventLog.Entity.RemoveComponent<ActiveCombatLogComponent>();
        }

        private TeamColor GetColor(Optional<TeamNode> team, BattleUserNode battleUser) => 
            !team.IsPresent() ? (!battleUser.Entity.HasComponent<ColorInBattleComponent>() ? TeamColor.NONE : battleUser.Entity.GetComponent<ColorInBattleComponent>().TeamColor) : team.Get().colorInBattle.TeamColor;

        private Color GetTeamColor(Optional<TeamNode> team, BattleUserNode battleUser, CombatEventLogNode combatEventLog) => 
            CombatEventLogUtil.GetTeamColor(this.GetColor(team, battleUser), combatEventLog.combatEventLog);

        [OnEventFire]
        public void KillStreakBattleLog(KillStreakEvent e, SingleNode<TankIncarnationKillStatisticsComponent> node, [JoinByUser] UserNode userNode, [JoinByUser] BattleUserNode battleUser, [JoinByUser] RoundUserNode roundUser, [JoinByTeam] Optional<TeamNode> team, [JoinAll] CombatEventLogNode combatEventLog)
        {
            int kills = node.component.Kills;
            if ((kills >= 5) && ((kills % 5) == 0))
            {
                string messageText = CombatEventLogUtil.ApplyPlaceholder(combatEventLog.combatLogCommonMessages.KillStreakMessage.Replace("{killNum}", kills.ToString()), "{user}", userNode.userRank.Rank, userNode.userUid.Uid, this.GetTeamColor(team, battleUser, combatEventLog));
                combatEventLog.uiLog.UILog.AddMessage(messageText);
            }
        }

        [OnEventFire]
        public void NotifyAboutScheduledGold(GoldScheduledNotificationEvent evt, Node any, [JoinAll] CombatEventLogNode combatEventLog)
        {
            string messageText = !string.IsNullOrEmpty(evt.Sender) ? string.Format(combatEventLog.combatLogCommonMessages.UserGoldScheduledMessage, evt.Sender) : combatEventLog.combatLogCommonMessages.GoldScheduledMessage;
            combatEventLog.uiLog.UILog.AddMessage(messageText);
        }

        [OnEventFire]
        public void NotifyAboutTakenGold(GoldTakenNotificationEvent e, BattleUserNode battleUser, [JoinByUser] UserNode user, [JoinByUser] RoundUserNode roundUser, [JoinByTeam] Optional<TeamNode> team, [JoinAll] CombatEventLogNode combatEventLog)
        {
            string messageText = CombatEventLogUtil.ApplyPlaceholder(combatEventLog.combatLogCommonMessages.GoldTakenMessage, "{user}", user.userRank.Rank, user.userUid.Uid, this.GetTeamColor(team, battleUser, combatEventLog));
            combatEventLog.uiLog.UILog.AddMessage(messageText);
        }

        [OnEventFire]
        public void NotifyAboutUserExit(NodeRemoveEvent e, BattleUserNode battleUser, [JoinByUser, Context] UserNode user, [JoinByUser] BattleUserNode battleUser2Team, [JoinByTeam] Optional<TeamNode> team, [JoinAll] CombatEventLogNode combatEventLog)
        {
            string messageText = CombatEventLogUtil.ApplyPlaceholder(combatEventLog.combatLogCommonMessages.UserLeaveBattleMessage, "{user}", user.userRank.Rank, user.userUid.Uid, this.GetTeamColor(team, battleUser, combatEventLog));
            combatEventLog.uiLog.UILog.AddMessage(messageText);
        }

        [OnEventFire]
        public void OnUserAdded(NodeAddedEvent e, DMBattleUserNode battleUser, [JoinByUser] NotSelfUserNode userNode, [JoinByBattle] SingleNode<DMComponent> dm, [JoinAll] CombatEventLogNode combatEventLogNode)
        {
            this.AddUserAddedMessage(userNode, battleUser.colorInBattle.TeamColor, combatEventLogNode);
        }

        [OnEventFire]
        public void OnUserAdded(NodeAddedEvent e, TeamBattleUserNode teamBattleUserNode, [JoinByUser] NotSelfUserNode userNode, TeamBattleUserNode teamBattleUser2Node, [JoinByTeam] TeamNode teamNode, [JoinAll] CombatEventLogNode combatEventLogNode)
        {
            this.AddUserAddedMessage(userNode, teamNode.colorInBattle.TeamColor, combatEventLogNode);
        }

        [OnEventFire]
        public void RedirectEventToTargetOnTargetDeath(KillEvent e, BattleUserNode battleUser, [JoinByUser] UserNode user, BattleUserNode battleUser2Team, [JoinByTeam] Optional<TeamNode> team)
        {
            ShowMessageAfterKilledEvent eventInstance = new ShowMessageAfterKilledEvent {
                KillerUserUid = user.userUid.Uid,
                killerRank = user.userRank.Rank,
                killerTeam = this.GetColor(team, battleUser),
                killerItem = e.KillerMarketItem.Id
            };
            base.ScheduleEvent(eventInstance, e.Target);
        }

        [OnEventFire]
        public void ShowKilledMessage(ShowMessageAfterKilledEvent e, TankNode victimTank, [JoinByUser] UserNode victimUser, [JoinByUser] BattleUserNode user, TankNode victimTank2Team, [JoinByTeam] Optional<TeamNode> team, [JoinAll] CombatEventLogNode combatEventLog)
        {
            string messageText = CombatEventLogUtil.ApplyPlaceholder(CombatEventLogUtil.ApplyPlaceholder(combatEventLog.combatLogCommonMessages.KillMessage, "{victim}", victimUser.userRank.Rank, victimUser.userUid.Uid, this.GetTeamColor(team, user, combatEventLog)), "{killer}", e.killerRank, e.KillerUserUid, CombatEventLogUtil.GetTeamColor(e.killerTeam, combatEventLog.combatEventLog)).Replace("{killItem}", e.killerItem.ToString());
            combatEventLog.uiLog.UILog.AddMessage(messageText);
        }

        [OnEventFire]
        public void ShowMessageOnUserSuicides(SelfDestructionBattleUserEvent e, BattleUserNode user, [JoinByUser] UserNode suicidedUser, BattleUserNode user2Team, [JoinByTeam] Optional<TeamNode> team, [JoinAll] CombatEventLogNode combatEventLog)
        {
            Color color = this.GetTeamColor(team, user, combatEventLog);
            string messageText = CombatEventLogUtil.ApplyPlaceholder(combatEventLog.combatLogCommonMessages.SuicideMessage, "{user}", suicidedUser.userRank.Rank, suicidedUser.userUid.Uid, color);
            combatEventLog.uiLog.UILog.AddMessage(messageText);
        }

        public class BattleUserNode : Node
        {
            public BattleUserComponent battleUser;
            public BattleGroupComponent battleGroup;
            public UserGroupComponent userGroup;
            public UserInBattleAsTankComponent userInBattleAsTank;
        }

        public class CombatEventLogNode : Node
        {
            public CombatLogCommonMessagesComponent combatLogCommonMessages;
            public CombatEventLogComponent combatEventLog;
            public UILogComponent uiLog;
            public ActiveCombatLogComponent activeCombatLog;
        }

        public class DMBattleUserNode : CombatEventLogSystem.BattleUserNode
        {
            public ColorInBattleComponent colorInBattle;
        }

        [Not(typeof(ActiveCombatLogComponent))]
        public class InactiveCombatLogNode : Node
        {
            public CombatLogCommonMessagesComponent combatLogCommonMessages;
            public CombatEventLogComponent combatEventLog;
            public UILogComponent uiLog;
        }

        [Not(typeof(SelfUserComponent))]
        public class NotSelfUserNode : CombatEventLogSystem.UserNode
        {
        }

        public class RoundUserNode : Node
        {
            public RoundUserComponent roundUser;
            public UserGroupComponent userGroup;
        }

        public class SelfBattleUserNode : Node
        {
            public BattleUserComponent battleUser;
            public UserGroupComponent userGroup;
            public SelfBattleUserComponent selfBattleUser;
        }

        public class TankNode : Node
        {
            public UserGroupComponent userGroup;
            public TankComponent tank;
        }

        public class TeamBattleUserNode : CombatEventLogSystem.BattleUserNode
        {
            public TeamGroupComponent teamGroup;
        }

        public class TeamNode : Node
        {
            public TeamComponent team;
            public TeamGroupComponent teamGroup;
            public ColorInBattleComponent colorInBattle;
        }

        public class UserNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
            public UserUidComponent userUid;
            public UserRankComponent userRank;
        }
    }
}

