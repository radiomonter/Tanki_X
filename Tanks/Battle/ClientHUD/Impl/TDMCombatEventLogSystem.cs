namespace Tanks.Battle.ClientHUD.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientHUD.API;

    public class TDMCombatEventLogSystem : ECSSystem
    {
        [OnEventFire]
        public void OnBattleStart(NodeAddedEvent e, CombatEventLogNode combatEventLog, BattleUserNode battleUser, [JoinByBattle] SingleNode<TDMComponent> tdm)
        {
            string battleStartMessage = combatEventLog.combatLogTDMMessages.BattleStartMessage;
            combatEventLog.uiLog.UILog.AddMessage(battleStartMessage);
        }

        public class BattleUserNode : Node
        {
            public BattleUserComponent battleUser;
            public UserInBattleAsTankComponent userInBattleAsTank;
            public UserGroupComponent userGroup;
            public BattleGroupComponent battleGroup;
            public SelfBattleUserComponent selfBattleUser;
        }

        public class CombatEventLogNode : Node
        {
            public CombatLogTDMMessagesComponent combatLogTDMMessages;
            public CombatEventLogComponent combatEventLog;
            public ActiveCombatLogComponent activeCombatLog;
            public UILogComponent uiLog;
        }
    }
}

