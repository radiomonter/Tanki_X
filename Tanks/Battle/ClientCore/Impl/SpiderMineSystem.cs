namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class SpiderMineSystem : ECSSystem
    {
        [OnEventFire]
        public void AcceptNewTarget(NodeAddedEvent e, SpiderActiveWithTargetNode activeSpider)
        {
            Entity target = activeSpider.unitTarget.Target;
            if (target.HasComponent<RigidbodyComponent>())
            {
                SpiderMineConfigComponent spiderMineConfig = activeSpider.spiderMineConfig;
                SpiderAnimatorComponent spiderAnimator = activeSpider.spiderAnimator;
                spiderAnimator.Speed = spiderMineConfig.Speed;
                spiderAnimator.Acceleration = spiderMineConfig.Acceleration;
                spiderAnimator.SetTarget(target.GetComponent<RigidbodyComponent>().Rigidbody);
                spiderAnimator.StartRuning();
            }
        }

        [OnEventFire]
        public void Activate(NodeAddedEvent e, SpiderActiveNode activeSpider)
        {
            activeSpider.spiderAnimator.StartActivation();
        }

        [OnEventFire]
        public void AddCTFEvaluator(NodeAddedEvent evt, [Combine] SpiderSelfActiveNode spider, [JoinByBattle] SingleNode<CTFComponent> battle)
        {
            spider.Entity.AddComponent<CTFTargetEvaluatorComponent>();
        }

        [OnEventFire]
        public void AddTeamEvaluator(NodeAddedEvent evt, [Combine] SpiderSelfActiveNode spider, [JoinByBattle] SingleNode<TeamBattleComponent> battle)
        {
            spider.Entity.AddComponent<TeamTargetEvaluatorComponent>();
        }

        [OnEventFire]
        public void Explosion(TriggerEnterEvent e, RemoteTankNode targetTank, SpiderActiveNode spider, [JoinByTank] SingleNode<SelfTankComponent> tank)
        {
            MineUtil.ExecuteSplashExplosion(spider.Entity, tank.Entity, spider.rigidbody.Rigidbody.transform.position);
        }

        [OnEventFire]
        public void InitSelf(NodeAddedEvent e, SpiderSelfActiveNode spider)
        {
            spider.Entity.AddComponent<UnitTargetingComponent>();
            spider.rigidbody.Rigidbody.transform.GetChild(0).GetComponentInChildren<Rigidbody>(true).gameObject.AddComponent<MinePhysicsTriggerBehaviour>().TriggerEntity = spider.Entity;
        }

        [OnEventFire]
        public void Instantiate(NodeAddedEvent e, [Combine] SpiderInstantiatedNode spider, SingleNode<MapInstanceComponent> map)
        {
            GameObject gameObject = spider.effectInstance.GameObject;
            spider.Entity.AddComponent(new RigidbodyComponent(gameObject.GetComponent<Rigidbody>()));
            Node[] nodes = new Node[] { spider, map };
            base.NewEvent(new InitMinePlacingTransformEvent(spider.effectInstance.GameObject.transform.position)).AttachAll(nodes).Schedule();
        }

        [OnEventFire]
        public void RemoveTarget(NodeRemoveEvent e, SpiderActiveWithTargetNode activeSpider)
        {
            SpiderAnimatorComponent spiderAnimator = activeSpider.spiderAnimator;
            spiderAnimator.SetTarget(null);
            spiderAnimator.StartIdle();
        }

        [OnEventComplete]
        public void StopUnit(SelfSplashHitEvent e, SpiderActiveNode spider)
        {
            spider.Entity.RemoveComponent<UnitReadyComponent>();
        }

        [OnEventFire]
        public void UpdateTarget(UpdateEvent e, SpiderActiveWithTargetNode activeSpider, [JoinByTank] SingleNode<SelfTankComponent> tank)
        {
            Entity target = activeSpider.unitTarget.Target;
            if (!(target.Alive && target.HasComponent<TankActiveStateComponent>()))
            {
                activeSpider.Entity.RemoveComponent<UnitTargetComponent>();
            }
        }

        public class RemoteTankNode : Node
        {
            public RemoteTankComponent remoteTank;
            public EnemyComponent enemy;
            public RigidbodyComponent rigidbody;
        }

        public class SpiderActiveNode : SpiderMineSystem.SpiderInstantiatedNode
        {
            public UnitReadyComponent unitReady;
            public RigidbodyComponent rigidbody;
            public SpiderAnimatorComponent spiderAnimator;
            public EffectActiveComponent effectActive;
        }

        public class SpiderActiveWithTargetNode : SpiderMineSystem.SpiderActiveNode
        {
            public UnitTargetComponent unitTarget;
            public SpiderMineConfigComponent spiderMineConfig;
        }

        public class SpiderEffectNode : Node
        {
            public SpiderMineEffectComponent spiderMineEffect;
            public UnitMoveComponent unitMove;
            public MineConfigComponent mineConfig;
        }

        public class SpiderInstantiatedNode : SpiderMineSystem.SpiderEffectNode
        {
            public EffectInstanceComponent effectInstance;
        }

        public class SpiderSelfActiveNode : SpiderMineSystem.SpiderActiveNode
        {
            public SelfComponent self;
        }
    }
}

