namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class LifestealGraphicsEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void InitLifestealEffect(NodeAddedEvent e, TankNode tank, [JoinByTank, Context] WeaponNode weapon)
        {
            tank.Entity.RemoveComponentIfPresent<LifestealGraphicsEffectReadyComponent>();
            tank.lifestealGraphicsEffect.InitRepairGraphicsEffect(new HealingGraphicEffectInputs(tank.Entity, tank.baseRenderer.Renderer as SkinnedMeshRenderer), new WeaponHealingGraphicEffectInputs(weapon.Entity, weapon.weaponVisualRoot.transform, weapon.baseRenderer.Renderer as SkinnedMeshRenderer), tank.tankSoundRoot.SoundRootTransform, tank.visualMountPoint.MountPoint);
            tank.Entity.AddComponent<LifestealGraphicsEffectReadyComponent>();
        }

        [OnEventComplete]
        public void PlayLifestealEffect(PlayLifestealGraphicsEffectEvent evt, ReadyTankInvisibilityEffectActivationStateNode tank)
        {
            tank.lifestealGraphicsEffect.StartEffect(tank.tankInvisibilityEffectUnity.InvisibilityEffectTransitionShader, 0f);
        }

        [OnEventComplete]
        public void PlayLifestealEffect(PlayLifestealGraphicsEffectEvent evt, ReadyTankInvisibilityEffectDeactivationStateNode tank)
        {
            tank.lifestealGraphicsEffect.StartEffect(tank.tankInvisibilityEffectUnity.InvisibilityEffectTransitionShader, 0f);
        }

        [OnEventComplete]
        public void PlayLifestealEffect(PlayLifestealGraphicsEffectEvent evt, ReadyTankInvisibilityEffectIdleStateNode tank)
        {
            tank.lifestealGraphicsEffect.StartEffect(tank.tankShader.TransparentShader, 0f);
        }

        [OnEventComplete]
        public void PlayLifestealEffect(PlayLifestealGraphicsEffectEvent evt, ReadyTankInvisibilityEffectWorkingStateNode tank)
        {
            tank.lifestealGraphicsEffect.StartEffect(tank.tankInvisibilityEffectUnity.InvisibilityEffectShader, 0f);
        }

        [OnEventFire]
        public void PlayLifestealEffect(TriggerEffectExecuteEvent e, SingleNode<LifestealEffectComponent> effect, [JoinByTank] ActiveTankReadyNode tank)
        {
            base.ScheduleEvent<PlayLifestealGraphicsEffectEvent>(tank);
        }

        [OnEventFire]
        public void PrepareLifeStealEffect(PlayLifestealGraphicsEffectEvent evt, ActiveTankReadyNode tank)
        {
            base.ScheduleEvent(new AddTankShaderEffectEvent(ClientGraphicsConstants.LIFESTEAL_EFFECT, false), tank);
        }

        [OnEventFire]
        public void StopEmergencyProtectionGraphicEfect(PlayLifestealGraphicsEffectEvent evt, EmergencyProtectionTankReadyNode tank)
        {
            tank.simpleEmergencyProtectionGraphicEffect.StopEffect();
        }

        [OnEventFire]
        public void StopHealingGraphicEfect(PlayLifestealGraphicsEffectEvent evt, HealingTankReadyNode tank)
        {
            tank.healingGraphicEffect.StopEffect();
        }

        [OnEventFire]
        public void StopLifestealGraphicsEffect(NodeRemoveEvent e, ActiveTankReadyNode tank)
        {
            tank.lifestealGraphicsEffect.StopEffect();
        }

        [OnEventFire]
        public void StopLifestealGraphicsEffect(StopLifestealTankShaderEffectEvent e, SingleNode<TankComponent> tank)
        {
            base.ScheduleEvent(new StopTankShaderEffectEvent(ClientGraphicsConstants.LIFESTEAL_EFFECT, false), tank);
        }

        public class ActiveTankReadyNode : LifestealGraphicsEffectSystem.TankReadyNode
        {
            public TankActiveStateComponent tankActiveState;
        }

        public class EmergencyProtectionTankReadyNode : LifestealGraphicsEffectSystem.TankReadyNode
        {
            public SimpleEmergencyProtectionGraphicEffectComponent simpleEmergencyProtectionGraphicEffect;
            public EmergencyProtectionTankShaderEffectReadyComponent emergencyProtectionTankShaderEffectReady;
        }

        public class HealingTankReadyNode : LifestealGraphicsEffectSystem.TankReadyNode
        {
            public HealingGraphicEffectComponent healingGraphicEffect;
        }

        public class PlayLifestealGraphicsEffectEvent : Event
        {
        }

        public class ReadyTankInvisibilityEffectActivationStateNode : LifestealGraphicsEffectSystem.ActiveTankReadyNode
        {
            public TankInvisibilityEffectActivationStateComponent tankInvisibilityEffectActivationState;
        }

        public class ReadyTankInvisibilityEffectDeactivationStateNode : LifestealGraphicsEffectSystem.ActiveTankReadyNode
        {
            public TankInvisibilityEffectDeactivationStateComponent tankInvisibilityEffectDeactivationState;
        }

        public class ReadyTankInvisibilityEffectIdleStateNode : LifestealGraphicsEffectSystem.ActiveTankReadyNode
        {
            public TankInvisibilityEffectIdleStateComponent tankInvisibilityEffectIdleState;
        }

        public class ReadyTankInvisibilityEffectWorkingStateNode : LifestealGraphicsEffectSystem.ActiveTankReadyNode
        {
            public TankInvisibilityEffectWorkingStateComponent tankInvisibilityEffectWorkingState;
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public LifestealGraphicsEffectComponent lifestealGraphicsEffect;
            public TankOpaqueShaderBlockersComponent tankOpaqueShaderBlockers;
            public RendererInitializedComponent rendererInitialized;
            public TankGroupComponent tankGroup;
            public TankShaderComponent tankShader;
            public BaseRendererComponent baseRenderer;
            public TankInvisibilityEffectUnityComponent tankInvisibilityEffectUnity;
            public HealingGraphicsEffectReadyComponent healingGraphicsEffectReady;
            public TankVisualRootComponent tankVisualRoot;
            public TankSoundRootComponent tankSoundRoot;
            public VisualMountPointComponent visualMountPoint;
        }

        public class TankReadyNode : LifestealGraphicsEffectSystem.TankNode
        {
            public LifestealGraphicsEffectReadyComponent lifestealGraphicsEffectReady;
        }

        public class WeaponNode : Node
        {
            public WeaponComponent weapon;
            public WeaponVisualRootComponent weaponVisualRoot;
            public BaseRendererComponent baseRenderer;
            public RendererInitializedComponent rendererInitialized;
            public TankGroupComponent tankGroup;
        }
    }
}

