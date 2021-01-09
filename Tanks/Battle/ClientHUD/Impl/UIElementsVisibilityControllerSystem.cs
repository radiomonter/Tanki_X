namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientHUD.API;

    public class UIElementsVisibilityControllerSystem : ECSSystem
    {
        private readonly string TANK_INACTIVE_PREREQUISITE = "TANK_INACTIVE_PREREQUISITE";
        private readonly string TAB_PRESSED_PREREQUISITE = "TAB_PRESSED_PREREQUISITE";
        private readonly string VISIBLE_PERIOD_PREREQUISITE = "VISIBLE_PERIOD_PREREQUISITE";
        private readonly string NORMALLY_VISIBLE_PREREQUISITE = "NORMALLY_VISIBLE_PREREQUISITE";
        private readonly string TAB_PRESSED_WHILE_IN_ACTIVE_STATE = "TAB_PRESSED_WHILE_IN_ACTIVE_STATE";

        [OnEventFire]
        public void AddNormallyVisiblePrerequisite(NodeAddedEvent e, NormallyVisibleNode uiElement)
        {
            uiElement.visibilityPrerequisites.AddShowPrerequisite(this.NORMALLY_VISIBLE_PREREQUISITE, false);
        }

        [OnEventFire]
        public void HideUIElementIfTabPressed(NodeAddedEvent e, HideWhileTabPressedNode uiElement)
        {
            uiElement.visibilityPrerequisites.AddHidePrerequisite(this.TAB_PRESSED_PREREQUISITE, true);
        }

        [OnEventFire]
        public void HideUIElementIfTabUnpressed(NodeRemoveEvent e, ShowWhileTabPressedNode showWhileTabPressed)
        {
            showWhileTabPressed.visibilityPrerequisites.RemoveShowPrerequisite(this.TAB_PRESSED_PREREQUISITE, true);
        }

        [OnEventFire]
        public void HideUIElementIfTabUnpressedInActiveState(NodeRemoveEvent e, [Combine] ShowWhileTabPressedActiveStateNode showWhileTabPressed, ActiveSelfTankNode activeTank)
        {
            showWhileTabPressed.visibilityPrerequisites.RemoveShowPrerequisite(this.TAB_PRESSED_WHILE_IN_ACTIVE_STATE, true);
        }

        [OnEventFire]
        public void HideUIElementIfTankActivated(NodeAddedEvent e, ActiveSelfTankNode activeSelfTank, [Combine, JoinAll] ShowWhileTankInactiveNode uiElement)
        {
            uiElement.visibilityPrerequisites.RemoveShowPrerequisite(this.TANK_INACTIVE_PREREQUISITE, false);
        }

        [OnEventFire]
        public void OnStopVisibilityPeriod(StopVisiblePeriodEvent e, SingleNode<VisibilityPrerequisitesComponent> uiElement)
        {
            uiElement.component.RemoveShowPrerequisite(this.VISIBLE_PERIOD_PREREQUISITE, false);
        }

        [OnEventFire]
        public void RemoveNormallyVisiblePrerequisite(NodeRemoveEvent e, NormallyVisibleNode uiElement)
        {
            uiElement.visibilityPrerequisites.RemoveShowPrerequisite(this.NORMALLY_VISIBLE_PREREQUISITE, false);
        }

        [OnEventFire]
        public void ScheduleElementConcealment(StartVisiblePeriodEvent e, SingleNode<VisibilityPrerequisitesComponent> uiElement)
        {
            uiElement.component.AddShowPrerequisite(this.VISIBLE_PERIOD_PREREQUISITE, false);
            base.NewEvent<StopVisiblePeriodEvent>().Attach(uiElement).ScheduleDelayed(e.DurationInSec);
        }

        [OnEventFire]
        public void ShowUIElementIfTabPressed(NodeAddedEvent e, ShowWhileTabPressedNode uiElement)
        {
            uiElement.visibilityPrerequisites.AddShowPrerequisite(this.TAB_PRESSED_PREREQUISITE, true);
        }

        [OnEventFire]
        public void ShowUIElementIfTabPressed(NodeRemoveEvent e, HideWhileTabPressedNode uiElement)
        {
            uiElement.visibilityPrerequisites.RemoveHidePrerequisite(this.TAB_PRESSED_PREREQUISITE, true);
        }

        [OnEventFire]
        public void ShowUIElementIfTabPressedInActiveState(NodeAddedEvent e, [Combine] ShowWhileTabPressedActiveStateNode uiElement, ActiveSelfTankNode activeTank)
        {
            uiElement.visibilityPrerequisites.AddShowPrerequisite(this.TAB_PRESSED_WHILE_IN_ACTIVE_STATE, true);
        }

        [OnEventFire]
        public void ShowUIElementIfTankDead(NodeAddedEvent e, DeadSelfTankNode deadSelfTank, [Combine] ShowWhileTankInactiveNode uiElement)
        {
            uiElement.visibilityPrerequisites.AddShowPrerequisite(this.TANK_INACTIVE_PREREQUISITE, false);
        }

        [OnEventFire]
        public void ShowUIElementIfTankSemiActive(NodeAddedEvent e, SemiActiveSelfTankNode semiActiveSelfTank, [Combine] ShowWhileTankInactiveNode uiElement)
        {
            uiElement.visibilityPrerequisites.AddShowPrerequisite(this.TANK_INACTIVE_PREREQUISITE, false);
        }

        [OnEventFire]
        public void ShowUIElementIfTankSpawn(NodeAddedEvent e, SpawnSelfTankNode spawnSelfTank, [Combine] ShowWhileTankInactiveNode uiElement)
        {
            uiElement.visibilityPrerequisites.AddShowPrerequisite(this.TANK_INACTIVE_PREREQUISITE, false);
        }

        public class ActiveSelfTankNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public SelfTankComponent selfTank;
        }

        public class DeadSelfTankNode : Node
        {
            public TankDeadStateComponent tankDeadState;
            public SelfTankComponent selfTank;
        }

        public class HideWhileTabPressedNode : Node
        {
            public TabPressedComponent tabPressed;
            public VisibilityPrerequisitesComponent visibilityPrerequisites;
            public HideWhileTabPressedComponent hideWhileTabPressed;
        }

        public class NormallyVisibleNode : Node
        {
            public VisibilityPrerequisitesComponent visibilityPrerequisites;
            public NormallyVisibleComponent normallyVisible;
        }

        public class SemiActiveSelfTankNode : Node
        {
            public TankSemiActiveStateComponent tankSemiActiveState;
            public SelfTankComponent selfTank;
        }

        public class ShowWhileTabPressedActiveStateNode : Node
        {
            public TabPressedComponent tabPressed;
            public VisibilityPrerequisitesComponent visibilityPrerequisites;
            public ShowWhileTabPressedActiveStateComponent showWhileTabPressedActiveState;
        }

        public class ShowWhileTabPressedNode : Node
        {
            public TabPressedComponent tabPressed;
            public VisibilityPrerequisitesComponent visibilityPrerequisites;
            public ShowWhileTabPressedComponent showWhileTabPressed;
        }

        public class ShowWhileTankInactiveNode : Node
        {
            public VisibilityPrerequisitesComponent visibilityPrerequisites;
            public ShowWhileTankInactiveComponent showWhileTankInactive;
        }

        public class SpawnSelfTankNode : Node
        {
            public TankSpawnStateComponent tankSpawnState;
            public SelfTankComponent selfTank;
        }
    }
}

