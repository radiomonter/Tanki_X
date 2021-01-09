namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class EMPEffectSystem : ECSSystem
    {
        private bool CheckBodyInRadius(Vector3 center, float radius, Vector3 targetPosition, Collider collider, float yOffset = 0f)
        {
            RaycastHit hit;
            Vector3 vector2 = (targetPosition + new Vector3(0f, yOffset, 0f)) - center;
            Ray ray = new Ray(center, vector2.normalized);
            return collider.Raycast(ray, out hit, radius);
        }

        private void CollectTargetsForEMP(CollectTargetsInRadius e, TankNode tank, IEnumerable<RemoteTankNode> otherTanks)
        {
            Vector3 position = tank.rigidbody.Rigidbody.position;
            e.Targets = new List<Entity>();
            foreach (RemoteTankNode node in otherTanks)
            {
                Vector3 targetPosition = node.rigidbody.Rigidbody.position;
                Collider boundsCollider = node.tankColliders.BoundsCollider;
                SkinnedMeshRenderer renderer = node.baseRenderer.Renderer as SkinnedMeshRenderer;
                if ((renderer != null) && this.CheckBodyInRadius(position, e.Radius, targetPosition, boundsCollider, renderer.localBounds.extents.y))
                {
                    e.Targets.Add(node.Entity);
                }
            }
        }

        [OnEventFire]
        public void CollectTargetsForEMPEffectInNonTeamBattle(NodeAddedEvent e, EMPEffectNode emp, [JoinByTank] SelfTankNonTeamNode selfTank, [JoinByBattle] NonTeamBattleNode battle, [JoinByBattle] ICollection<RemoteTankNode> otherTanks)
        {
            CollectTargetsInRadius eventInstance = new CollectTargetsInRadius {
                Radius = emp.empEffect.Radius
            };
            Node[] nodes = new Node[] { emp, selfTank, battle };
            base.NewEvent(eventInstance).AttachAll(nodes).Schedule();
        }

        [OnEventFire]
        public void CollectTargetsForEMPEffectInNonTeamBattle(CollectTargetsInRadius e, EffectNode any, SelfTankNonTeamNode selfTank, NonTeamBattleNode battle, [JoinByBattle] ICollection<RemoteTankNode> otherTanks)
        {
            this.CollectTargetsForEMP(e, selfTank, otherTanks);
        }

        [OnEventFire]
        public void CollectTargetsForEMPEffectInTeamBattle(NodeAddedEvent e, EMPEffectNode emp, [JoinByTank] SelfTankTeamNode selfTank, [JoinByTeam] TeamNode selfTeam, [JoinByBattle] TeamBattleNode battle, [JoinByBattle, Combine] TeamNode team)
        {
            if (!team.Entity.Equals(selfTeam.Entity))
            {
                CollectTargetsInRadius eventInstance = new CollectTargetsInRadius {
                    Radius = emp.empEffect.Radius
                };
                Node[] nodes = new Node[] { emp, selfTank, battle, team };
                base.NewEvent(eventInstance).AttachAll(nodes).Schedule();
            }
        }

        [OnEventFire]
        public void CollectTargetsForEMPEffectInTeamBattle(CollectTargetsInRadius e, EffectNode any, SelfTankTeamNode selfTank, TeamBattleNode battle, TeamNode team, [JoinByTeam] ICollection<RemoteTankNode> otherTanks)
        {
            this.CollectTargetsForEMP(e, selfTank, otherTanks);
        }

        [OnEventFire]
        public void EmptySlotLocked(NodeAddedEvent e, SingleNode<SlotLockedByEMPComponent> node)
        {
        }

        [OnEventComplete]
        public void SendEmpTargetsToServer(CollectTargetsInRadius evt, EMPEffectNode emp, SelfTankNode tank)
        {
            ApplyTargetsForEMPEffectEvent eventInstance = new ApplyTargetsForEMPEffectEvent {
                Targets = evt.Targets.ToArray()
            };
            base.ScheduleEvent<SynchronizeSelfTankPositionBeforeEffectEvent>(tank);
            base.ScheduleEvent(eventInstance, emp);
        }

        public class BattleNode : Node
        {
            public BattleComponent battle;
            public BattleGroupComponent battleGroup;
        }

        public class EffectNode : Node
        {
            public EffectComponent effect;
        }

        public class EMPEffectNode : Node
        {
            public EMPEffectComponent empEffect;
            public TankGroupComponent tankGroup;
        }

        [Not(typeof(TeamGroupComponent))]
        public class NonTeamBattleNode : EMPEffectSystem.BattleNode
        {
        }

        public class RemoteTankNode : EMPEffectSystem.TankNode
        {
            public RemoteTankComponent remoteTank;
        }

        public class SelfTankNode : EMPEffectSystem.TankNode
        {
            public SelfTankComponent selfTank;
        }

        [Not(typeof(TeamGroupComponent))]
        public class SelfTankNonTeamNode : EMPEffectSystem.SelfTankNode
        {
        }

        public class SelfTankTeamNode : EMPEffectSystem.SelfTankNode
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
        }

        public class TeamBattleNode : EMPEffectSystem.BattleNode
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

