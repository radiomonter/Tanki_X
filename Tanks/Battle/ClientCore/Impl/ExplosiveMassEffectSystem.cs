namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class ExplosiveMassEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void CollectTargetsInNonTeamBattle(NodeAddedEvent e, ExplosiveMassEffectNode effect, [JoinByTank] SelfTankNonTeamNode selfTank, [JoinByBattle] NonTeamBattleNode battle, [JoinByBattle] ICollection<RemoteTankNode> otherTanks)
        {
            CollectTargetsInRadius eventInstance = new CollectTargetsInRadius {
                Radius = effect.explosiveMassEffect.Radius
            };
            Node[] nodes = new Node[] { effect, selfTank, battle };
            base.NewEvent(eventInstance).AttachAll(nodes).ScheduleDelayed(((float) effect.explosiveMassEffect.Delay) / 1000f);
        }

        [OnEventFire]
        public void CollectTargetsInTeamBattle(NodeAddedEvent e, ExplosiveMassEffectNode effect, [JoinByTank] SelfTankTeamNode selfTank, [JoinByTeam] TeamNode selfTeam, [JoinByBattle] TeamBattleNode battle, [JoinByBattle, Combine] TeamNode team)
        {
            if (!team.Entity.Equals(selfTeam.Entity))
            {
                CollectTargetsInRadius eventInstance = new CollectTargetsInRadius {
                    Radius = effect.explosiveMassEffect.Radius
                };
                Node[] nodes = new Node[] { effect, selfTank, battle, team };
                base.NewEvent(eventInstance).AttachAll(nodes).ScheduleDelayed(((float) effect.explosiveMassEffect.Delay) / 1000f);
            }
        }

        [OnEventFire]
        public void GetIncarnation(GetTankIncarnationEvent e, TankNode tank, [JoinByTank] SingleNode<TankIncarnationComponent> incarnation)
        {
            e.TankIncarnation = incarnation.Entity;
        }

        [OnEventFire]
        public void PlayEffect(NodeAddedEvent e, ExplosiveMassEffectNode effectNode, [JoinByTank] TankNode tank)
        {
            GameObject explosiveMassEffect = tank.moduleVisualEffectObjects.ExplosiveMassEffect;
            if (!explosiveMassEffect.activeInHierarchy)
            {
                explosiveMassEffect.transform.position = tank.rigidbody.RigidbodyTransform.position;
                explosiveMassEffect.SetActive(true);
            }
            TankFallEvent eventInstance = new TankFallEvent {
                FallingPower = 100f,
                FallingType = TankFallingType.NOTHING
            };
            base.NewEvent(eventInstance).Attach(tank).ScheduleDelayed(((float) effectNode.explosiveMassEffect.Delay) / 1000f);
        }

        [OnEventComplete]
        public void SendTargetsToServer(CollectTargetsInRadius evt, ExplosiveMassEffectNode effect, SelfTankNode tank)
        {
            SelfHitEvent eventInstance = new SelfHitEvent {
                Targets = new List<HitTarget>()
            };
            foreach (Entity entity in evt.Targets)
            {
                Entity tankIncarnation = entity.SendEvent<GetTankIncarnationEvent>(new GetTankIncarnationEvent()).TankIncarnation;
                if (tankIncarnation != null)
                {
                    HitTarget item = new HitTarget {
                        Entity = entity,
                        LocalHitPoint = Vector3.zero,
                        HitDirection = Vector3.zero,
                        HitDistance = 0f,
                        IncarnationEntity = tankIncarnation,
                        TargetPosition = Vector3.zero
                    };
                    eventInstance.Targets.Add(item);
                }
            }
            base.ScheduleEvent<SynchronizeSelfTankPositionBeforeEffectEvent>(tank);
            base.ScheduleEvent(eventInstance, effect);
        }

        public class BattleNode : Node
        {
            public BattleComponent battle;
            public BattleGroupComponent battleGroup;
        }

        public class ExplosiveMassEffectNode : Node
        {
            public ExplosiveMassEffectComponent explosiveMassEffect;
            public EffectComponent effect;
        }

        [Not(typeof(TeamGroupComponent))]
        public class NonTeamBattleNode : ExplosiveMassEffectSystem.BattleNode
        {
        }

        public class RemoteTankNode : ExplosiveMassEffectSystem.TankNode
        {
            public RemoteTankComponent remoteTank;
        }

        public class SelfTankNode : ExplosiveMassEffectSystem.TankNode
        {
            public SelfTankComponent selfTank;
        }

        [Not(typeof(TeamGroupComponent))]
        public class SelfTankNonTeamNode : ExplosiveMassEffectSystem.SelfTankNode
        {
        }

        public class SelfTankTeamNode : ExplosiveMassEffectSystem.SelfTankNode
        {
            public TeamGroupComponent teamGroup;
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

        public class TeamBattleNode : ExplosiveMassEffectSystem.BattleNode
        {
            public TeamBattleComponent teamBattle;
        }

        public class TeamNode : Node
        {
            public TeamComponent team;
            public TeamGroupComponent teamGroup;
            public BattleGroupComponent battleGroup;
        }
    }
}

