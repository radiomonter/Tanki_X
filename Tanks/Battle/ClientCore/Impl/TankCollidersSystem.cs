namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class TankCollidersSystem : ECSSystem
    {
        [CompilerGenerated]
        private static Func<Collider, GameObject> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<GameObject, bool> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<VisualTriggerMarkerComponent, GameObject> <>f__am$cache2;
        [CompilerGenerated]
        private static Func<Collider, bool> <>f__am$cache3;

        private List<GameObject> CollectMeshColliders(GameObject root)
        {
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = c => c.gameObject;
            }
            return root.GetComponentsInChildren<VisualTriggerMarkerComponent>(true).Select<VisualTriggerMarkerComponent, GameObject>(<>f__am$cache2).ToList<GameObject>();
        }

        private List<Collider> CollectTankToStaticColliders(GameObject root)
        {
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = go => go.transform.parent.gameObject.name == TankCollidersUnityComponent.TANK_TO_STATIC_COLLIDER_NAME;
            }
            return root.GetComponentsInChildren<Collider>(true).Where<Collider>(<>f__am$cache3).ToList<Collider>();
        }

        private List<GameObject> CollectTargetColliders(GameObject root)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = c => c.gameObject;
            }
            <>f__am$cache1 ??= go => (go.name == TankCollidersUnityComponent.TARGETING_COLLIDER_NAME);
            return root.GetComponentsInChildren<Collider>(true).Select<Collider, GameObject>(<>f__am$cache0).Where<GameObject>(<>f__am$cache1).ToList<GameObject>();
        }

        [OnEventFire]
        public void DisableTankToTankBeforeDisappear(NodeAddedEvent e, DeadTransparentTankNode tank)
        {
            tank.tankColliders.TankToTankCollider.enabled = false;
            tank.rigidbody.Rigidbody.gameObject.layer = Layers.INVISIBLE_PHYSICS;
        }

        [OnEventComplete]
        public void EnableBoundsCollider(NodeAddedEvent evt, RemoteActiveTankCollidersNode node)
        {
            node.tankColliders.BoundsCollider.enabled = true;
        }

        [OnEventComplete]
        public void EnableBoundsCollider(NodeAddedEvent evt, RemoteDeadTankCollidersNode node)
        {
            node.tankColliders.BoundsCollider.enabled = true;
        }

        [OnEventFire]
        public void PrepareColliders(NodeAddedEvent evt, TankNode tankNode)
        {
            TankCollidersComponent tankColliders = new TankCollidersComponent();
            TankCollidersUnityComponent tankCollidersUnity = tankNode.tankCollidersUnity;
            tankColliders.BoundsCollider = tankCollidersUnity.GetBoundsCollider();
            tankColliders.TankToTankCollider = tankCollidersUnity.GetTankToTankCollider();
            tankColliders.VisualTriggerColliders = this.CollectMeshColliders(tankNode.assembledTank.AssemblyRoot);
            tankColliders.TargetingColliders = this.CollectTargetColliders(tankNode.assembledTank.AssemblyRoot);
            tankColliders.TankToStaticColliders = this.CollectTankToStaticColliders(tankNode.assembledTank.AssemblyRoot);
            tankColliders.Extends = (Vector3) (Quaternion.Inverse(tankColliders.TankToTankCollider.transform.rotation) * tankColliders.TankToTankCollider.bounds.extents);
            tankColliders.TankToStaticTopCollider = tankCollidersUnity.GetTankToStaticTopCollider();
            this.SetCollidersLayer(tankColliders, Layers.INVISIBLE_PHYSICS, Layers.INVISIBLE_PHYSICS, Layers.INVISIBLE_PHYSICS);
            this.SetGameObjectLayers(tankColliders.TankToStaticColliders, Layers.TANK_TO_STATIC);
            tankNode.Entity.AddComponent(tankColliders);
        }

        [OnEventFire]
        public void SetCollidersForActiveTank(NodeAddedEvent evt, ActiveTankNode tankNode)
        {
            this.SetCollidersLayer(tankNode.tankColliders, Layers.TANK_TO_TANK, Layers.TARGET, Layers.TANK_PART_VISUAL);
            tankNode.tankColliders.TankToTankCollider.enabled = true;
            tankNode.tankColliders.BoundsCollider.enabled = true;
        }

        [OnEventFire]
        public void SetCollidersForDeadTank(NodeAddedEvent evt, DeadTankNode tankNode)
        {
            this.SetCollidersLayer(tankNode.tankColliders, Layers.TANK_TO_TANK, Layers.DEAD_TARGET, Layers.TANK_PART_VISUAL);
            tankNode.tankColliders.TankToTankCollider.enabled = false;
        }

        [OnEventFire]
        public void SetCollidersForNewTank(NodeAddedEvent evt, TankNewStateNode tankNode)
        {
            if (tankNode.assembledTank.AssemblyRoot != null)
            {
                this.SetCollidersLayer(tankNode.tankColliders, Layers.INVISIBLE_PHYSICS, Layers.INVISIBLE_PHYSICS, Layers.INVISIBLE_PHYSICS);
                tankNode.tankColliders.TankToTankCollider.enabled = false;
            }
        }

        [OnEventFire]
        public void SetCollidersForSpawnTank(NodeAddedEvent evt, TankSpawnStateNode tankNode)
        {
            this.SetCollidersLayer(tankNode.tankColliders, Layers.INVISIBLE_PHYSICS, Layers.INVISIBLE_PHYSICS, Layers.INVISIBLE_PHYSICS);
            tankNode.tankColliders.TankToTankCollider.enabled = false;
        }

        private void SetCollidersLayer(TankCollidersComponent tankColliders, int tankToTankLayer, int targetingLayer, int meshColliderLayer)
        {
            tankColliders.TankToTankCollider.gameObject.layer = tankToTankLayer;
            this.SetGameObjectLayers(tankColliders.TargetingColliders, targetingLayer);
            this.SetGameObjectLayers(tankColliders.VisualTriggerColliders, meshColliderLayer);
        }

        private void SetGameObjectLayers(IEnumerable<Collider> colliders, int layer)
        {
            foreach (Collider collider in colliders)
            {
                collider.gameObject.layer = layer;
            }
        }

        private void SetGameObjectLayers(IEnumerable<GameObject> colliders, int layer)
        {
            foreach (GameObject obj2 in colliders)
            {
                obj2.layer = layer;
            }
        }

        [OnEventFire]
        public void SetupBoundsColliderForRemoteTank(NodeAddedEvent evt, RemoteTankCollidersNode node)
        {
            node.tankColliders.BoundsCollider.isTrigger = false;
            node.tankColliders.BoundsCollider.gameObject.layer = Layers.REMOTE_TANK_BOUNDS;
        }

        [OnEventFire]
        public void SetupBoundsColliderForSelfTank(NodeAddedEvent evt, SelfSemiActiveTankNode node)
        {
            node.tankColliders.BoundsCollider.gameObject.layer = Layers.SELF_SEMIACTIVE_TANK_BOUNDS;
        }

        [OnEventFire]
        public void SetupBoundsColliderForSelfTank(NodeRemoveEvent evt, SingleNode<TankSemiActiveStateComponent> semiActive, [JoinSelf] SelfTankNode node)
        {
            node.tankColliders.BoundsCollider.gameObject.layer = Layers.SELF_TANK_BOUNDS;
        }

        public class ActiveTankNode : TankCollidersSystem.TankCollidersNode
        {
            public TankActiveStateComponent tankActiveState;
        }

        public class DeadTankNode : TankCollidersSystem.TankCollidersNode
        {
            public TankDeadStateComponent tankDeadState;
        }

        public class DeadTransparentTankNode : TankCollidersSystem.TankCollidersNode
        {
            public TankDeadStateComponent tankDeadState;
            public TransparencyTransitionComponent transparencyTransition;
        }

        public class RemoteActiveTankCollidersNode : TankCollidersSystem.RemoteTankCollidersNode
        {
            public TankActiveStateComponent tankActiveState;
        }

        public class RemoteDeadTankCollidersNode : TankCollidersSystem.RemoteTankCollidersNode
        {
            public TankDeadStateComponent tankDeadState;
        }

        public class RemoteTankCollidersNode : TankCollidersSystem.TankCollidersNode
        {
            public RemoteTankComponent remoteTank;
        }

        public class SelfSemiActiveTankNode : TankCollidersSystem.SelfTankNode
        {
            public TankSemiActiveStateComponent tankSemiActiveState;
        }

        public class SelfTankNode : TankCollidersSystem.TankCollidersNode
        {
            public SelfTankComponent selfTank;
        }

        public class TankCollidersNode : TankCollidersSystem.TankNode
        {
            public TankCollidersComponent tankColliders;
        }

        public class TankNewStateNode : TankCollidersSystem.TankCollidersNode
        {
            public TankNewStateComponent tankNewState;
        }

        public class TankNode : Node
        {
            public AssembledTankComponent assembledTank;
            public HullInstanceComponent hullInstance;
            public RigidbodyComponent rigidbody;
            public TankGroupComponent tankGroup;
            public TankCollidersUnityComponent tankCollidersUnity;
        }

        public class TankSpawnStateNode : TankCollidersSystem.TankCollidersNode
        {
            public TankSpawnStateComponent tankSpawnState;
        }
    }
}

