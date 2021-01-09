namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientHUD.API;
    using Tanks.Lobby.ClientNavigation.API;

    public class RoundSpecificVisibilityControllerSystem : ECSSystem
    {
        private static readonly string ROUND_RESTARTING_SHOW_PREREQUISITE = "ROUND_RESTARTING_SHOW_PREREQUISITE";
        private static readonly string ROUND_RESTARTING_HIDE_PREREQUISITE = "ROUND_RESTARTING_HIDE_PREREQUISITE";

        [OnEventFire]
        public void HideUIWhenRoundEntersInactiveState(NodeAddedEvent e, SingleNode<RoundRestartingStateComponent> round, [JoinAll, Combine] RoundRestartingInvisibleNode roundRestartingInvisible)
        {
            roundRestartingInvisible.visibilityPrerequisites.AddHidePrerequisite(ROUND_RESTARTING_HIDE_PREREQUISITE, false);
        }

        [OnEventFire]
        public void HideUIWhenRoundLeavesInactiveState(NodeRemoveEvent e, SingleNode<RoundRestartingStateComponent> round, [JoinAll, Combine] RoundRestartingVisibleNode roundRestartingVisible)
        {
            roundRestartingVisible.visibilityPrerequisites.RemoveShowPrerequisite(ROUND_RESTARTING_SHOW_PREREQUISITE, false);
        }

        [OnEventFire]
        public void ShowUIWhenRoundEntersInactiveState(NodeAddedEvent e, SingleNode<RoundRestartingStateComponent> round, [JoinAll, Combine] RoundRestartingVisibleNode roundRestartingVisible)
        {
            roundRestartingVisible.visibilityPrerequisites.AddShowPrerequisite(ROUND_RESTARTING_SHOW_PREREQUISITE, false);
        }

        [OnEventFire]
        public void ShowUIWhenRoundLeavesInactiveState(NodeRemoveEvent e, SingleNode<RoundRestartingStateComponent> round, [JoinAll, Combine] RoundRestartingInvisibleNode roundRestartingInvisible)
        {
            roundRestartingInvisible.visibilityPrerequisites.RemoveHidePrerequisite(ROUND_RESTARTING_HIDE_PREREQUISITE, false);
        }

        public class RoundRestartingInvisibleNode : Node
        {
            public HideWhileRoundIsRestartingComponent hideWhileRoundIsRestarting;
            public VisibilityPrerequisitesComponent visibilityPrerequisites;
        }

        public class RoundRestartingVisibleNode : Node
        {
            public ShowWhileRoundIsRestartingComponent showWhileRoundIsRestarting;
            public VisibilityPrerequisitesComponent visibilityPrerequisites;
        }

        public class ScreenNode : Node
        {
            public ScreenComponent screen;
            public BattleScreenComponent battleScreen;
            public BattleGroupComponent battleGroup;
        }
    }
}

