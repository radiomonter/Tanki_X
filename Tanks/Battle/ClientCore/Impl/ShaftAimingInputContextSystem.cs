namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class ShaftAimingInputContextSystem : ECSSystem
    {
        [OnEventFire]
        public void SwitchContextToAiming(NodeAddedEvent evt, ShaftAimingWorkActivationStateNode weapon, [JoinAll, Mandatory] SingleNode<BattleHUDESMComponent> battleHUD)
        {
            InputManager.DeactivateContext(BasicContexts.BATTLE_CONTEXT);
            InputManager.ClearActions();
            InputManager.ActivateContext(ShaftAimingContexts.AIMING_TARGETING);
            battleHUD.component.Esm.ChangeState<BattleHUDStates.ShaftAimingState>();
        }

        [OnEventFire]
        public void SwitchContextToBattle(NodeAddedEvent evt, AimingIdleStateNode weapon, [JoinAll] SingleNode<BattleHUDESMComponent> battleHUD)
        {
            InputManager.DeactivateContext(ShaftAimingContexts.AIMING_TARGETING);
            InputManager.ClearActions();
            InputManager.ActivateContext(BasicContexts.BATTLE_CONTEXT);
            battleHUD.component.Esm.ChangeState<BattleHUDStates.ActionsState>();
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class AimingIdleStateNode : Node
        {
            public ShaftIdleStateComponent shaftIdleState;
            public ShaftStateControllerComponent shaftStateController;
            public TankGroupComponent tankGroup;
        }

        public class ShaftAimingWorkActivationStateNode : Node
        {
            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;
            public ShaftStateControllerComponent shaftStateController;
            public TankGroupComponent tankGroup;
        }
    }
}

