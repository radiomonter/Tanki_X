﻿namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;

    public class NitroEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void InitNitroEffect(NodeAddedEvent e, [Combine] InitialTankNode tank, SingleNode<SoundListenerBattleStateComponent> soundListener, SingleNode<SupplyEffectSettingsComponent> settings)
        {
            if (!tank.Entity.HasComponent<TankDeadStateComponent>())
            {
                tank.nitroEffect.InitEffect(settings.component);
                tank.Entity.AddComponent<NitroEffectReadyComponent>();
            }
        }

        [OnEventFire]
        public void StartNitroEffect(NodeAddedEvent e, SpeedEffectNode effect, [Context, JoinByTank] ReadyTankNode tank)
        {
            tank.nitroEffect.Play();
        }

        [OnEventFire]
        public void StopNitroEffect(NodeRemoveEvent evt, SpeedEffectNode effect, [JoinByTank] ReadyTankNode tank)
        {
            tank.nitroEffect.Stop();
        }

        public class InitialTankNode : Node
        {
            public AnimationPreparedComponent animationPrepared;
            public NitroEffectComponent nitroEffect;
            public TankGroupComponent tankGroup;
        }

        public class ReadyTankNode : NitroEffectSystem.InitialTankNode
        {
            public NitroEffectReadyComponent nitroEffectReady;
        }

        public class SpeedEffectNode : Node
        {
            public TankGroupComponent tankGroup;
            public TurboSpeedEffectComponent turboSpeedEffect;
        }
    }
}

