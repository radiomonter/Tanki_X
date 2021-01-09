namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;

    public class StreamWeaponParticleSystem : ECSSystem
    {
        [OnEventFire]
        public void Init(NodeAddedEvent evt, StreamWeaponEffectInitNode node, [Context, JoinByTank] AssembledActivatedTankNode tank, [JoinAll] SingleNode<StreamWeaponSettingsComponent> settings)
        {
            node.streamEffect.Init(node.muzzlePoint);
            node.streamEffect.Instance.ApplySettings(settings.component);
            node.Entity.AddComponent<StreamEffectReadyComponent>();
        }

        [OnEventFire]
        public void StartParticleSystems(NodeAddedEvent e, WorkingNode weapon)
        {
            weapon.streamEffect.Instance.Play();
        }

        [OnEventFire]
        public void StopParticleSystems(NodeRemoveEvent e, WorkingNode weapon)
        {
            StreamEffectBehaviour instance = weapon.streamEffect.Instance;
            if (instance)
            {
                instance.Stop();
            }
        }

        [OnEventFire]
        public void StopParticleSystems(NodeRemoveEvent e, AssembledActivatedTankNode tankNode, [JoinByTank] WorkingNode weapon)
        {
            StreamEffectBehaviour instance = weapon.streamEffect.Instance;
            if (instance)
            {
                instance.Stop();
            }
        }

        public class AssembledActivatedTankNode : Node
        {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankFriendlyEnemyStatusDefinedComponent tankFriendlyEnemyStatusDefined;
            public TankGroupComponent tankGroup;
        }

        public class StreamWeaponEffectInitNode : Node
        {
            public StreamEffectComponent streamEffect;
            public MuzzlePointComponent muzzlePoint;
            public TankGroupComponent tankGroup;
        }

        public class WorkingNode : Node
        {
            public StreamEffectReadyComponent streamEffectReady;
            public StreamEffectComponent streamEffect;
            public StreamWeaponWorkingComponent streamWeaponWorking;
            public WeaponUnblockedComponent weaponUnblocked;
        }
    }
}

