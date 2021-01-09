namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class ExternalImpactEffectSystem : ECSSystem
    {
        private float SPLASH_CENTER_HEIGHT = 0.4f;

        [OnEventFire]
        public void CollectSplashTarget(StartSplashEffectEvent e, SplashEffectNode effect, [JoinByTank] SelfTankNode selfTank, [JoinByTeam] ICollection<RemoteTankNode> remoteTanks)
        {
            StaticHit staticHit = new StaticHit {
                Normal = Vector3.up,
                Position = selfTank.rigidbody.Rigidbody.position + (this.SPLASH_CENTER_HEIGHT * (selfTank.rigidbody.Rigidbody.rotation * Vector3.up))
            };
            SplashHitData splashHit = SplashHitData.CreateSplashHitData(new List<HitTarget>(), staticHit, effect.Entity);
            HashSet<Entity> set = new HashSet<Entity> {
                selfTank.Entity
            };
            splashHit.ExcludedEntityForSplashHit = set;
            if (!effect.splashEffect.CanTargetTeammates)
            {
                foreach (RemoteTankNode node in remoteTanks)
                {
                    splashHit.ExcludedEntityForSplashHit.Add(node.Entity);
                }
            }
            base.ScheduleEvent<SendTankMovementEvent>(selfTank);
            base.ScheduleEvent(new CollectSplashTargetsEvent(splashHit), effect);
        }

        [OnEventFire]
        public void EnableEffect(NodeAddedEvent e, ExternalImpactEffectNode effectNode, [JoinByTank] TankNode tank, [JoinAll] SelfTankNode selfTank)
        {
            GameObject externalImpactEffect = tank.moduleVisualEffectObjects.ExternalImpactEffect;
            if (!externalImpactEffect.activeInHierarchy)
            {
                externalImpactEffect.transform.position = tank.rigidbody.RigidbodyTransform.position;
                externalImpactEffect.SetActive(true);
            }
            base.ScheduleEvent<StartSplashEffectEvent>(effectNode);
            TankFallEvent eventInstance = new TankFallEvent {
                FallingPower = 100f,
                FallingType = TankFallingType.NOTHING
            };
            base.ScheduleEvent(eventInstance, selfTank);
        }

        public class EffectNode : Node
        {
            public EffectComponent effect;
        }

        public class ExternalImpactEffectNode : ExternalImpactEffectSystem.SplashEffectNode
        {
            public ExternalImpactEffectComponent externalImpactEffect;
        }

        public class RemoteTankNode : ExternalImpactEffectSystem.TankNode
        {
            public RemoteTankComponent remoteTank;
        }

        public class SelfTankNode : ExternalImpactEffectSystem.TankNode
        {
            public SelfTankComponent selfTank;
        }

        public class SplashEffectNode : ExternalImpactEffectSystem.EffectNode
        {
            public SplashEffectComponent splashEffect;
            public SplashWeaponComponent splashWeapon;
        }

        public class TankNode : Node
        {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankActiveStateComponent tankActiveState;
            public TankGroupComponent tankGroup;
            public BattleGroupComponent battleGroup;
            public RigidbodyComponent rigidbody;
            public BaseRendererComponent baseRenderer;
            public TankCollidersComponent tankColliders;
            public ModuleVisualEffectObjectsComponent moduleVisualEffectObjects;
        }
    }
}

