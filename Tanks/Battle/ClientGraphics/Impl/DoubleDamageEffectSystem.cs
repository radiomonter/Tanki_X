namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;

    public class DoubleDamageEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void InitDoubleDamageEffect(NodeAddedEvent e, [Combine] InitialWeaponNode weapon, [Combine, JoinByTank] TankNode tank, SingleNode<SoundListenerBattleStateComponent> soundListener, SingleNode<SupplyEffectSettingsComponent> settings)
        {
            if (!tank.Entity.HasComponent<TankDeadStateComponent>())
            {
                weapon.doubleDamageEffect.InitEffect(settings.component);
                weapon.Entity.AddComponent<DoubleDamageEffectReadyComponent>();
            }
        }

        [OnEventFire]
        public void PlayDoubleDamageEffect(NodeAddedEvent e, DamageEffectNode effect, [Context, JoinByTank] ReadyWeaponNode weapon)
        {
            weapon.doubleDamageEffect.Play();
        }

        [OnEventFire]
        public void Reset(NodeAddedEvent e, TankIncarnationNode incarnation, [JoinByTank] InitializedWeaponNode weapon)
        {
            weapon.doubleDamageEffect.Reset();
        }

        [OnEventFire]
        public void StopDoubleDamageEffect(NodeRemoveEvent e, DamageEffectNode effect, [JoinByTank] ReadyWeaponNode weapon)
        {
            weapon.doubleDamageEffect.Stop();
        }

        public class DamageEffectNode : Node
        {
            public TankGroupComponent tankGroup;
            public DamageEffectComponent damageEffect;
        }

        public class InitializedWeaponNode : DoubleDamageEffectSystem.InitialWeaponNode
        {
            public DoubleDamageEffectReadyComponent doubleDamageEffectReady;
        }

        public class InitialWeaponNode : Node
        {
            public AnimationPreparedComponent animationPrepared;
            public TankGroupComponent tankGroup;
            public DoubleDamageEffectComponent doubleDamageEffect;
        }

        public class ReadyWeaponNode : DoubleDamageEffectSystem.InitialWeaponNode
        {
            public DoubleDamageEffectReadyComponent doubleDamageEffectReady;
        }

        public class TankIncarnationNode : Node
        {
            public TankIncarnationComponent tankIncarnation;
            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node
        {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankGroupComponent tankGroup;
        }
    }
}

