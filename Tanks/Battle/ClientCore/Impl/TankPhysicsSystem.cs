namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class TankPhysicsSystem : ECSSystem
    {
        private static float MAX_DEPENETRATION_VELOCITY = 5f;

        [OnEventFire]
        public void ApplyDepenetrationForce(FixedUpdateEvent evt, SelfPhysicNode selfPhysics, [JoinByBattle] ICollection<RemotePhysicNode> remotePhysics)
        {
            Rigidbody rigidbody = selfPhysics.rigidbody.Rigidbody;
            BoxCollider tankToTankCollider = (BoxCollider) selfPhysics.tankColliders.TankToTankCollider;
            bool flag = false;
            foreach (RemotePhysicNode node in remotePhysics)
            {
                Rigidbody rigidbody2 = node.rigidbody.Rigidbody;
                BoxCollider collider2 = (BoxCollider) node.tankColliders.TankToTankCollider;
                Bounds bounds = tankToTankCollider.bounds;
                if (bounds.Intersects(collider2.bounds))
                {
                    Entity entity = node.Entity;
                    if ((entity.HasComponent<TankActiveStateComponent>() || entity.HasComponent<TankDeadStateComponent>()) && DepenetrationForce.ApplyDepenetrationForce(rigidbody, tankToTankCollider, rigidbody2, collider2))
                    {
                        flag = true;
                    }
                }
            }
            rigidbody.maxDepenetrationVelocity = !flag ? MAX_DEPENETRATION_VELOCITY : 0f;
        }

        [OnEventFire]
        public void ApplyMoveHelperForces(FixedUpdateEvent evt, PhysicNode selfPhysics)
        {
            Rigidbody rigidbody = selfPhysics.rigidbody.Rigidbody;
            Transform rigidbodyTransform = selfPhysics.rigidbody.RigidbodyTransform;
            if (rigidbody != null)
            {
                Vector3 vector = ((BoxCollider) selfPhysics.tankColliders.TankToTankCollider).size * 0.5f;
                Vector3 origin = rigidbodyTransform.TransformPoint(new Vector3(-vector.x, vector.y * 0.5f, vector.z));
                Vector3 vector3 = rigidbodyTransform.TransformPoint(new Vector3(vector.x, vector.y * 0.5f, vector.z));
                float magnitude = Vector3.Project(rigidbody.velocity, rigidbodyTransform.forward).magnitude;
                float length = 0.1f + (magnitude * 0.25f);
                bool flag = this.StaticCollided(origin, rigidbodyTransform.forward, length);
                bool flag2 = !this.StaticCollided(origin + (rigidbodyTransform.right * 0.3f), rigidbodyTransform.forward, length * 1.2f);
                bool flag3 = this.StaticCollided(vector3, rigidbodyTransform.forward, length);
                bool flag4 = !this.StaticCollided(vector3 - (rigidbodyTransform.right * 0.3f), rigidbodyTransform.forward, length * 1.2f);
                if (Vector3.Dot(rigidbody.velocity.normalized, rigidbodyTransform.forward) > 0f)
                {
                    if (flag && (flag2 && !flag3))
                    {
                        rigidbody.AddTorqueSafe(((new Vector3(0f, 1f, 0f) * magnitude) * rigidbody.mass) * 2f);
                    }
                    if (!flag && (flag4 && flag3))
                    {
                        rigidbody.AddTorqueSafe(((-new Vector3(0f, 1f, 0f) * magnitude) * rigidbody.mass) * 2f);
                    }
                }
            }
        }

        public bool ApplyWingForce(Rigidbody body, Vector3 wingDirection, Vector3 strength)
        {
            float num = -Vector3.Dot(wingDirection, Vector3.up);
            if (num <= 0f)
            {
                return false;
            }
            body.AddRelativeTorqueSafe((strength * num) * num);
            return true;
        }

        [OnEventFire]
        public void ApplyWingForces(FixedUpdateEvent evt, PhysicNode selfPhysics, [JoinAll] MapNode mapPhysic)
        {
            Rigidbody rigidbody = selfPhysics.rigidbody.Rigidbody;
            Transform rigidbodyTransform = selfPhysics.rigidbody.RigidbodyTransform;
            if (rigidbody != null)
            {
                float num = Vector3.Dot(rigidbodyTransform.up, Vector3.up);
                if (num < 0.9f)
                {
                    float num2 = Physics.gravity.y / -10f;
                    Vector3 normalized = rigidbodyTransform.InverseTransformDirection(rigidbody.angularVelocity).normalized;
                    float num3 = 0.5f;
                    if (num <= 0f)
                    {
                        if ((num3 > 0f) && (rigidbody.angularVelocity.magnitude > 0.2f))
                        {
                            float num4 = ((num3 * rigidbody.mass) * num2) * Mathf.Sign(normalized.z);
                            rigidbody.AddRelativeTorqueSafe(new Vector3(0f, 0f, 1f) * num4);
                        }
                    }
                    else
                    {
                        float num5 = (4f * rigidbody.mass) * num2;
                        if (!this.ApplyWingForce(rigidbody, rigidbodyTransform.right, new Vector3(0f, 0f, 1f) * num5))
                        {
                            this.ApplyWingForce(rigidbody, -rigidbodyTransform.right, new Vector3(0f, 0f, -1f) * num5);
                        }
                        float num6 = (4f * rigidbody.mass) * num2;
                        if (!this.ApplyWingForce(rigidbody, rigidbodyTransform.forward, new Vector3(-1f, 0f, 0f) * num6))
                        {
                            this.ApplyWingForce(rigidbody, -rigidbodyTransform.forward, new Vector3(1f, 0f, 0f) * num6);
                        }
                    }
                }
            }
        }

        [OnEventFire]
        public void AttachCollidersToTank(NodeAddedEvent e, PhysicsNode node)
        {
            Rigidbody rigidbody = node.rigidbody.Rigidbody;
            BoxCollider boundsCollider = node.tankCollidersUnity.GetBoundsCollider();
            this.SetInertiaTensor(ref rigidbody, boundsCollider.size, boundsCollider.center);
            node.Entity.AddComponent(new TriggerObjectComponent(boundsCollider.gameObject));
            rigidbody.maxDepenetrationVelocity = MAX_DEPENETRATION_VELOCITY;
            node.rigidbody.Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            rigidbody.WakeUp();
        }

        [OnEventFire]
        public void SetHighFriction(NodeAddedEvent e, DeadTankNode tank)
        {
            List<Collider> tankToStaticColliders = tank.tankColliders.TankToStaticColliders;
            int count = tankToStaticColliders.Count;
            for (int i = 0; i < count; i++)
            {
                Collider collider = tankToStaticColliders[i];
                collider.material = tank.tankCollidersUnity.highFrictionMaterial;
            }
        }

        private void SetInertiaTensor(ref Rigidbody rigidbody, Vector3 size, Vector3 center)
        {
            rigidbody.centerOfMass = center;
            rigidbody.inertiaTensor = new Vector3((size.y * size.y) + (size.z * size.z), (size.x * size.x) + (size.z * size.z), (size.y * size.y) + (size.x * size.x)) * (rigidbody.mass / 12f);
            rigidbody.inertiaTensorRotation = Quaternion.identity;
        }

        [OnEventFire]
        public void SetLowFriction(NodeRemoveEvent e, DeadTankNode tank)
        {
            List<Collider> tankToStaticColliders = tank.tankColliders.TankToStaticColliders;
            int count = tankToStaticColliders.Count;
            for (int i = 0; i < count; i++)
            {
                Collider collider = tankToStaticColliders[i];
                collider.material = tank.tankCollidersUnity.lowFrictionMaterial;
            }
            tank.tankColliders.TankToStaticTopCollider.material = tank.tankCollidersUnity.highFrictionMaterial;
        }

        private bool StaticCollided(Vector3 origin, Vector3 dir, float length)
        {
            RaycastHit hit;
            return (Physics.Raycast(origin, dir, out hit, length, LayerMasks.STATIC) && (Mathf.Abs(hit.normal.normalized.y) < 0.3f));
        }

        [OnEventFire]
        public void WakeUpActiveTankBody(NodeAddedEvent e, TankActiveStateNode node)
        {
            node.rigidbody.Rigidbody.WakeUp();
        }

        public class DeadTankNode : Node
        {
            public TankComponent tank;
            public TankCollidersComponent tankColliders;
            public TankCollidersUnityComponent tankCollidersUnity;
            public TankDeadStateComponent tankDeadState;
        }

        public class MapNode : Node
        {
            public MapInstanceComponent mapInstance;
        }

        public class PhysicNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public TankCollidersComponent tankColliders;
            public RigidbodyComponent rigidbody;
            public ChassisComponent chassis;
        }

        public class PhysicsNode : Node
        {
            public RigidbodyComponent rigidbody;
            public ChassisConfigComponent chassisConfig;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankCollidersUnityComponent tankCollidersUnity;
        }

        public class RemotePhysicNode : Node
        {
            public RemoteTankComponent remoteTank;
            public TankCollidersComponent tankColliders;
            public RigidbodyComponent rigidbody;
        }

        public class SelfPhysicNode : Node
        {
            public SelfTankComponent selfTank;
            public TankActiveStateComponent tankActiveState;
            public TankCollidersComponent tankColliders;
            public RigidbodyComponent rigidbody;
        }

        public class TankActiveStateNode : Node
        {
            public TankActiveStateComponent tankActiveState;
            public RigidbodyComponent rigidbody;
            public SelfTankComponent selfTank;
        }
    }
}

