namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class TankInvisibilityEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void ActivateInvisibilityEffect(ActivateTankInvisibilityEffectEvent evt, DeactivationTankInvisibilityEffectNode tank)
        {
            base.ScheduleEvent(new AddTankShaderEffectEvent(ClientGraphicsConstants.TANK_INVISIBILITY_EFFECT, false), tank);
            tank.tankInvisibilityEffectUnity.ActivateEffect();
        }

        [OnEventFire]
        public void ActivateInvisibilityEffect(ActivateTankInvisibilityEffectEvent evt, IdleTankInvisibilityEffectNode tank)
        {
            base.ScheduleEvent(new AddTankShaderEffectEvent(ClientGraphicsConstants.TANK_INVISIBILITY_EFFECT, false), tank);
            tank.tankInvisibilityEffectUnity.ActivateEffect();
        }

        [OnEventFire]
        public void ActivateTankInvisibilityGraphicEffect(NodeAddedEvent e, InvisibilityEffectNode effect, [Context, JoinByTank] InitializedTankInvisibilityEffectNode tank)
        {
            base.ScheduleEvent<ActivateTankInvisibilityEffectEvent>(tank);
        }

        [OnEventFire]
        public void DeactivateInvisibilityEffect(DeactivateTankInvisibilityEffectEvent evt, ActivationTankInvisibilityEffectNode tank)
        {
            tank.tankInvisibilityEffectUnity.DeactivateEffect();
        }

        [OnEventFire]
        public void DeactivateInvisibilityEffect(DeactivateTankInvisibilityEffectEvent evt, WorkingTankInvisibilityEffectNode tank)
        {
            tank.tankInvisibilityEffectUnity.DeactivateEffect();
        }

        [OnEventFire]
        public void DeactivateTankInvisibilityGraphicEffect(NodeRemoveEvent e, InvisibilityEffectNode effect, [JoinByTank] InitializedTankInvisibilityEffectNode tank)
        {
            base.ScheduleEvent<DeactivateTankInvisibilityEffectEvent>(tank);
        }

        [OnEventFire]
        public void InitTankVisibilityEffectStates(NodeAddedEvent evt, TankInvisibilityEffectNode tank, [JoinByTank, Context] WeaponNode weapon)
        {
            TankInvisibilityEffectESMComponent component = new TankInvisibilityEffectESMComponent();
            tank.Entity.AddComponent(component);
            component.Esm.AddState<TankInvisibilityEffectStates.TankInvisibilityEffectActivationState>();
            component.Esm.AddState<TankInvisibilityEffectStates.TankInvisibilityEffectDeactivationState>();
            component.Esm.AddState<TankInvisibilityEffectStates.TankInvisibilityEffectIdleState>();
            component.Esm.AddState<TankInvisibilityEffectStates.TankInvisibilityEffectWorkingState>();
            bool fullInvisibly = tank.Entity.HasComponent<EnemyComponent>();
            Renderer[] renderers = new Renderer[] { tank.baseRenderer.Renderer, weapon.baseRenderer.Renderer };
            tank.tankInvisibilityEffectUnity.ConfigureEffect(tank.Entity, fullInvisibly, renderers);
            tank.tankInvisibilityEffectUnity.ResetEffect();
            tank.Entity.AddComponent<TankInvisibilityEffectInitializedComponent>();
        }

        [OnEventFire]
        public void RemoveShaderEffect(NodeAddedEvent e, SingleNode<TankInvisibilityEffectIdleStateComponent> tank)
        {
            base.ScheduleEvent(new StopTankShaderEffectEvent(ClientGraphicsConstants.TANK_INVISIBILITY_EFFECT, false), tank);
        }

        [OnEventFire]
        public void ResetEffectOnTankIncarnation(NodeRemoveEvent e, TankIncarnationNode tankIncarnation, [JoinByTank] InitializedTankInvisibilityEffectNode tank)
        {
            tank.tankInvisibilityEffectUnity.ResetEffect();
        }

        [OnEventFire]
        public void SwitchTankInvisibilityEffectState(TankInvisibilityEffectSwitchStateEvent e, SingleNode<TankInvisibilityEffectESMComponent> tank)
        {
            tank.component.Esm.ChangeState(e.StateType);
        }

        public class ActivationTankInvisibilityEffectNode : TankInvisibilityEffectSystem.InitializedTankInvisibilityEffectNode
        {
            public TankInvisibilityEffectActivationStateComponent tankInvisibilityEffectActivationState;
        }

        public class DeactivationTankInvisibilityEffectNode : TankInvisibilityEffectSystem.InitializedTankInvisibilityEffectNode
        {
            public TankInvisibilityEffectDeactivationStateComponent tankInvisibilityEffectDeactivationState;
        }

        public class IdleTankInvisibilityEffectNode : TankInvisibilityEffectSystem.InitializedTankInvisibilityEffectNode
        {
            public TankInvisibilityEffectIdleStateComponent tankInvisibilityEffectIdleState;
        }

        public class InitializedTankInvisibilityEffectNode : TankInvisibilityEffectSystem.TankInvisibilityEffectNode
        {
            public TankInvisibilityEffectESMComponent tankInvisibilityEffectEsm;
            public TankInvisibilityEffectInitializedComponent tankInvisibilityEffectInitialized;
        }

        public class InvisibilityEffectNode : Node
        {
            public InvisibilityEffectComponent invisibilityEffect;
            public TankGroupComponent tankGroup;
        }

        public class TankIncarnationNode : Node
        {
            public TankIncarnationComponent tankIncarnation;
            public TankGroupComponent tankGroup;
        }

        public class TankInvisibilityEffectNode : Node
        {
            public TankGroupComponent tankGroup;
            public BaseRendererComponent baseRenderer;
            public TankVisualRootComponent tankVisualRoot;
            public TankInvisibilityEffectUnityComponent tankInvisibilityEffectUnity;
            public TankOpaqueShaderBlockersComponent tankOpaqueShaderBlockers;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
        }

        public class WeaponNode : Node
        {
            public BaseRendererComponent baseRenderer;
            public TankGroupComponent tankGroup;
            public WeaponComponent weapon;
        }

        public class WorkingTankInvisibilityEffectNode : TankInvisibilityEffectSystem.InitializedTankInvisibilityEffectNode
        {
            public TankInvisibilityEffectWorkingStateComponent tankInvisibilityEffectWorkingState;
        }
    }
}

