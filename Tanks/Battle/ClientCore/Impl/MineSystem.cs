namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class MineSystem : ECSSystem
    {
        private const float MINE_HALF_SIZE = 0.5f;

        [OnEventFire]
        public void ActivateMineTrigger(NodeAddedEvent e, ActiveMineNode mine, [JoinByTank, Context] EnemyTankNode tank)
        {
            Rigidbody componentInChildren = mine.effectInstance.GameObject.GetComponentInChildren<Rigidbody>();
            MeshCollider collider = componentInChildren.GetComponentInChildren<MeshCollider>();
            float num = 1f;
            Vector3 localScale = collider.transform.localScale;
            float x = (localScale.x * ((mine.mineEffectTriggeringArea.Radius + 0.5f) * 2f)) / num;
            collider.transform.localScale = new Vector3(x, localScale.y, x);
            componentInChildren.gameObject.AddComponent<MinePhysicsTriggerBehaviour>().TriggerEntity = mine.Entity;
        }

        [OnEventFire]
        public void PlaceMineOnGround(NodeAddedEvent e, MineInstanceNode mine)
        {
            MinePlacingTransformComponent minePlacingTransform = mine.minePlacingTransform;
            Transform transform = mine.effectInstance.GameObject.transform;
            if (!mine.minePlacingTransform.HasPlacingTransform)
            {
                transform.SetPositionSafe(mine.minePosition.Position);
            }
            else
            {
                transform.SetPositionSafe(minePlacingTransform.PlacingData.point);
                transform.SetRotationSafe(Quaternion.FromToRotation(Vector3.up, minePlacingTransform.PlacingData.normal));
            }
        }

        [OnEventFire]
        public void PrepareMinePosition(NodeAddedEvent evt, [Combine] MineNode mine, SingleNode<MapInstanceComponent> map)
        {
            Node[] nodes = new Node[] { mine, map };
            base.NewEvent(new InitMinePlacingTransformEvent(mine.minePosition.Position)).AttachAll(nodes).Schedule();
        }

        [OnEventFire]
        public void TriggerMine(TriggerEnterEvent e, ActiveMineNode mine, SingleNode<TankActiveStateComponent> tank)
        {
            base.ScheduleEvent<SendTankMovementEvent>(tank);
        }

        public class ActiveMineNode : MineSystem.MineInstanceNode
        {
            public EffectActiveComponent effectActive;
        }

        public class EnemyTankNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankComponent tank;
            public EnemyComponent enemy;
        }

        public class MineInstanceNode : MineSystem.MinePlacingTransformNode
        {
            public EffectInstanceComponent effectInstance;
            public TankGroupComponent tankGroup;
        }

        public class MineNode : Node
        {
            public MineEffectComponent mineEffect;
            public MinePositionComponent minePosition;
            public MineConfigComponent mineConfig;
            public MineEffectTriggeringAreaComponent mineEffectTriggeringArea;
        }

        public class MinePlacingTransformNode : MineSystem.MineNode
        {
            public MinePlacingTransformComponent minePlacingTransform;
        }
    }
}

