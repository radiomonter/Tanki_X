namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;

    public class DisbalanceInfoSystem : ECSSystem
    {
        private void ActivateEffect(Node node)
        {
            base.ScheduleEvent<ActivateMultikillNotificationEvent>(node);
        }

        [OnEventFire]
        public void RoundDisbalanced(NodeAddedEvent e, SingleNode<RoundDisbalancedComponent> roundDisbalanced, SingleNode<DisbalanceStartedWinNotificationComponent> winDisbalance, SingleNode<DisbalanceStartedLooseNotificationComponent> looseDisbalance, SingleNode<DisbalanceInfoComponent> disbalanceInfo, BattleUserNode user, [JoinByTeam] SingleNode<TeamColorComponent> team, [JoinByBattle] BattleNode battle)
        {
            TeamColor loser = roundDisbalanced.component.Loser;
            float time = roundDisbalanced.component.FinishTime.UnityTime - Date.Now.UnityTime;
            disbalanceInfo.component.Timer.Set(time, true);
            if (loser == team.component.TeamColor)
            {
                disbalanceInfo.component.ShowDisbalanceInfo(false, battle.battleMode.BattleMode);
                this.ActivateEffect(looseDisbalance);
            }
            else
            {
                disbalanceInfo.component.ShowDisbalanceInfo(true, battle.battleMode.BattleMode);
                this.ActivateEffect(winDisbalance);
            }
        }

        [OnEventFire]
        public void RoundReturnedToBalance(NodeRemoveEvent e, RoundNode roundDisbalanced, [JoinAll] SingleNode<DisbalanceInfoComponent> disbalanceInfo)
        {
            disbalanceInfo.component.HideDisbalanceInfo();
        }

        [OnEventFire]
        public void RoundReturnedToBalance(RoundBalanceRestoredEvent e, SingleNode<RoundDisbalancedComponent> roundDisbalanced, [JoinAll] SingleNode<SelfTankComponent> tank, [JoinByTeam] SingleNode<TeamColorComponent> team, [JoinAll] SingleNode<DisbalanceRemovedWinNotificationComponent> winDisbalance, [JoinAll] SingleNode<DisbalanceRemovedLooseNotificationComponent> looseDisbalance)
        {
            if (roundDisbalanced.component.Loser == team.component.TeamColor)
            {
                this.ActivateEffect(looseDisbalance);
            }
            else
            {
                this.ActivateEffect(winDisbalance);
            }
        }

        public class BattleNode : Node
        {
            public BattleModeComponent battleMode;
            public BattleComponent battle;
        }

        public class BattleUserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserReadyToBattleComponent userReadyToBattle;
        }

        public class RoundNode : Node
        {
            public RoundDisbalancedComponent roundDisbalanced;
            public RoundComponent round;
        }

        public class TankNode : Node
        {
            public SelfTankComponent selfTank;
        }
    }
}

