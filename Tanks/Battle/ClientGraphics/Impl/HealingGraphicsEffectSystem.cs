namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class HealingGraphicsEffectSystem : ECSSystem
    {
        private void InitRepairGraphicEffect(TankNode tank, WeaponNode weapon)
        {
            tank.Entity.RemoveComponentIfPresent<HealingGraphicsEffectReadyComponent>();
            tank.healingGraphicEffect.InitRepairGraphicsEffect(new HealingGraphicEffectInputs(tank.Entity, tank.baseRenderer.Renderer as SkinnedMeshRenderer), new WeaponHealingGraphicEffectInputs(weapon.Entity, weapon.weaponVisualRoot.transform, weapon.baseRenderer.Renderer as SkinnedMeshRenderer), tank.visualMountPoint.MountPoint, tank.tankSoundRoot.SoundRootTransform);
            tank.Entity.AddComponent<HealingGraphicsEffectReadyComponent>();
        }

        [OnEventFire]
        public void InitRepairGraphicEffect(NodeAddedEvent evt, TankActiveStateNode tank, [JoinByTank, Context] WeaponNode weapon)
        {
            this.InitRepairGraphicEffect(tank, weapon);
        }

        [OnEventComplete]
        public void StartRepairEffect(StartHealingGraphicsEffectEvent evt, ReadyTankInvisibilityEffectActivationStateNode tank)
        {
            tank.healingGraphicEffect.StartEffect(tank.tankInvisibilityEffectUnity.InvisibilityEffectTransitionShader, evt.Duration);
        }

        [OnEventComplete]
        public void StartRepairEffect(StartHealingGraphicsEffectEvent evt, ReadyTankInvisibilityEffectDeactivationStateNode tank)
        {
            tank.healingGraphicEffect.StartEffect(tank.tankInvisibilityEffectUnity.InvisibilityEffectTransitionShader, evt.Duration);
        }

        [OnEventComplete]
        public void StartRepairEffect(StartHealingGraphicsEffectEvent evt, ReadyTankInvisibilityEffectIdleStateNode tank)
        {
            tank.healingGraphicEffect.StartEffect(tank.tankShader.TransparentShader, evt.Duration);
        }

        [OnEventComplete]
        public void StartRepairEffect(StartHealingGraphicsEffectEvent evt, ReadyTankInvisibilityEffectWorkingStateNode tank)
        {
            tank.healingGraphicEffect.StartEffect(tank.tankInvisibilityEffectUnity.InvisibilityEffectShader, evt.Duration);
        }

        [OnEventFire]
        public void StartRepairEffect(StartHealingGraphicsEffectEvent evt, ReadyTankNode tank)
        {
            base.ScheduleEvent(new AddTankShaderEffectEvent(ClientGraphicsConstants.REPAIR_EFFECT, false), tank);
        }

        [OnEventFire]
        public void StartRepairEffect(NodeAddedEvent evt, HealingEffectNode effect, [JoinByTank, Context] ReadyTankNode tank, [JoinByTank, Context] WeaponNode weapon)
        {
            Node[] nodes = new Node[] { tank, weapon };
            base.NewEvent(new StartHealingGraphicsEffectEvent(((float) effect.durationConfig.Duration) / 1000f)).AttachAll(nodes).Schedule();
        }

        [OnEventFire]
        public void StopEffect(NodeRemoveEvent evt, ReadyTankActiveStateNode tank)
        {
            tank.healingGraphicEffect.StopEffect();
        }

        [OnEventFire]
        public void StopEffect(NodeRemoveEvent evt, HealingEffectNode effect, [JoinByTank] ReadyTankNode tank)
        {
            tank.healingGraphicEffect.StopEffect();
        }

        [OnEventFire]
        public void StopEmergencyProtectionEffect(StartHealingGraphicsEffectEvent evt, EmergencyProtectionReadyTankNode tank)
        {
            tank.simpleEmergencyProtectionGraphicEffect.StopEffect();
        }

        [OnEventFire]
        public void StopLifestealEffect(StartHealingGraphicsEffectEvent evt, LifestealTankReadyNode tank)
        {
            tank.lifestealGraphicsEffect.StopEffect();
        }

        [OnEventFire]
        public void StopRepairEffect(StopHealingGraphicsEffectEvent evt, ReadyTankNode tank)
        {
            base.ScheduleEvent(new StopTankShaderEffectEvent(ClientGraphicsConstants.REPAIR_EFFECT, false), tank);
        }

        public class EmergencyProtectionReadyTankNode : HealingGraphicsEffectSystem.ReadyTankNode
        {
            public SimpleEmergencyProtectionGraphicEffectComponent simpleEmergencyProtectionGraphicEffect;
            public EmergencyProtectionTankShaderEffectReadyComponent emergencyProtectionTankShaderEffectReady;
        }

        public class HealingEffectNode : Node
        {
            public HealingEffectComponent healingEffect;
            public DurationConfigComponent durationConfig;
            public TankGroupComponent tankGroup;
        }

        public class LifestealTankReadyNode : HealingGraphicsEffectSystem.ReadyTankNode
        {
            public LifestealGraphicsEffectComponent lifestealGraphicsEffect;
            public LifestealGraphicsEffectReadyComponent lifestealGraphicsEffectReady;
        }

        public class ReadyTankActiveStateNode : HealingGraphicsEffectSystem.TankNode
        {
            public TankActiveStateComponent tankActiveState;
            public HealingGraphicsEffectReadyComponent healingGraphicsEffectReady;
        }

        public class ReadyTankInvisibilityEffectActivationStateNode : HealingGraphicsEffectSystem.ReadyTankNode
        {
            public TankInvisibilityEffectActivationStateComponent tankInvisibilityEffectActivationState;
        }

        public class ReadyTankInvisibilityEffectDeactivationStateNode : HealingGraphicsEffectSystem.ReadyTankNode
        {
            public TankInvisibilityEffectDeactivationStateComponent tankInvisibilityEffectDeactivationState;
        }

        public class ReadyTankInvisibilityEffectIdleStateNode : HealingGraphicsEffectSystem.ReadyTankNode
        {
            public TankInvisibilityEffectIdleStateComponent tankInvisibilityEffectIdleState;
        }

        public class ReadyTankInvisibilityEffectWorkingStateNode : HealingGraphicsEffectSystem.ReadyTankNode
        {
            public TankInvisibilityEffectWorkingStateComponent tankInvisibilityEffectWorkingState;
        }

        public class ReadyTankNode : HealingGraphicsEffectSystem.TankNode
        {
            public HealingGraphicsEffectReadyComponent healingGraphicsEffectReady;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
        }

        public class TankActiveStateNode : HealingGraphicsEffectSystem.TankNode
        {
            public TankActiveStateComponent tankActiveState;
        }

        public class TankNode : Node
        {
            public TankInvisibilityEffectUnityComponent tankInvisibilityEffectUnity;
            public HealingGraphicEffectComponent healingGraphicEffect;
            public BaseRendererComponent baseRenderer;
            public RendererInitializedComponent rendererInitialized;
            public TankGroupComponent tankGroup;
            public TankShaderComponent tankShader;
            public TankSoundRootComponent tankSoundRoot;
            public TankOpaqueShaderBlockersComponent tankOpaqueShaderBlockers;
            public VisualMountPointComponent visualMountPoint;
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

