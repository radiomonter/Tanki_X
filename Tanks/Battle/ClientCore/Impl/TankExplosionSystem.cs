namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientEntrance.API;
    using UnityEngine;

    public class TankExplosionSystem : ECSSystem
    {
        [OnEventFire]
        public void AttachDetachedWeapon(NodeRemoveEvent e, TankNode tank, [JoinByTank, Context] TankIncarnationNode incornation, [JoinByTank] DetachedWeaponNode weapon)
        {
            this.StopCollide(weapon.weaponDetachCollider);
            WeaponVisualRootComponent weaponVisualRoot = weapon.weaponVisualRoot;
            weaponVisualRoot.transform.SetParent(tank.visualMountPoint.MountPoint, false);
            weaponVisualRoot.transform.localRotation = Quaternion.identity;
            weaponVisualRoot.transform.localPosition = Vector3.zero;
            this.StopCollide(weapon.weaponDetachCollider);
            weaponVisualRoot.VisualTriggerMarker.VisualTriggerMeshCollider.enabled = true;
            weapon.Entity.RemoveComponentIfPresent<DetachedWeaponComponent>();
        }

        [OnEventFire]
        public void DeathJump(SelfTankExplosionEvent e, SelfTankNode tank)
        {
            this.HullDeathJump(tank.rigidbody.Rigidbody);
            base.ScheduleEvent<SendTankMovementEvent>(tank);
            base.ScheduleEvent<DetachWeaponIfPossibleEvent>(tank);
        }

        private Rigidbody DetachWeapon(TankNode tank, UnblockedWeaponNode weapon)
        {
            Rigidbody rigidbody = tank.rigidbody.Rigidbody;
            WeaponVisualRootComponent weaponVisualRoot = weapon.weaponVisualRoot;
            WeaponDetachColliderComponent weaponDetachCollider = weapon.weaponDetachCollider;
            GameObject gameObject = weapon.weaponVisualRoot.gameObject;
            if (weapon.Entity.HasComponent<DetachedWeaponComponent>())
            {
                return rigidbody;
            }
            gameObject.transform.parent = tank.assembledTank.AssemblyRoot.transform;
            gameObject.layer = Layers.TANK_AND_STATIC;
            weaponVisualRoot.VisualTriggerMarker.VisualTriggerMeshCollider.enabled = false;
            MeshCollider collider = weaponDetachCollider.Collider;
            collider.enabled = true;
            Rigidbody rigidbody2 = weaponDetachCollider.Rigidbody;
            Bounds bounds = collider.sharedMesh.bounds;
            Vector3 center = bounds.center;
            center.z = 0f;
            this.SetInertiaTensor(rigidbody2, bounds.size, center);
            rigidbody2.mass = rigidbody.mass / 10f;
            rigidbody2.maxAngularVelocity = rigidbody.maxAngularVelocity;
            rigidbody2.maxDepenetrationVelocity = 0f;
            rigidbody2.angularDrag = 2f;
            rigidbody2.drag = 0f;
            rigidbody2.SetVelocitySafe(rigidbody.velocity);
            rigidbody2.SetAngularVelocitySafe(rigidbody.angularVelocity);
            rigidbody2.interpolation = RigidbodyInterpolation.Interpolate;
            rigidbody2.isKinematic = false;
            rigidbody2.detectCollisions = true;
            weapon.Entity.AddComponent<DetachedWeaponComponent>();
            return rigidbody2;
        }

        private void HullDeathJump(Rigidbody hullRigidbody)
        {
            Vector3 vector;
            hullRigidbody.velocity = vector = hullRigidbody.velocity + new Vector3(Random.Range((float) -1f, (float) 1f), Random.Range((float) 1f, (float) 3f), Random.Range((float) -1f, (float) 1f));
            hullRigidbody.SetVelocitySafe(vector);
            hullRigidbody.SetAngularVelocitySafe(hullRigidbody.angularVelocity + Vector3.Scale(Random.onUnitSphere, new Vector3(Random.Range((float) 1f, (float) 3f), Random.Range((float) 0.5f, (float) 1f), Random.Range((float) 1f, (float) 3f))));
        }

        [OnEventFire]
        public void RemoteDetachWeapon(DetachWeaponEvent e, RemoteTankNode tank, [JoinByTank] UnblockedWeaponNode weapon)
        {
            Rigidbody rigidbody = this.DetachWeapon(tank, weapon);
            rigidbody.SetVelocitySafe(e.Velocity);
            rigidbody.SetAngularVelocitySafe(e.AngularVelocity);
        }

        [OnEventFire]
        public void Reset(NodeAddedEvent e, TankIncarnationNode tankIncarnation, [JoinByTank] SingleNode<DetachedWeaponComponent> weapon)
        {
            weapon.Entity.RemoveComponent<DetachedWeaponComponent>();
        }

        [OnEventFire]
        public void SelfDetachWeapon(DetachWeaponIfPossibleEvent e, TankNode tank, [JoinByTank] UnblockedWeaponNode weapon)
        {
            Rigidbody rigidbody = this.DetachWeapon(tank, weapon);
            rigidbody.SetVelocitySafe(rigidbody.velocity + new Vector3(Random.Range((float) -1.5f, (float) 1.5f), Random.Range((float) 2f, (float) 3f), Random.Range((float) -1.5f, (float) 1.5f)));
            rigidbody.SetAngularVelocitySafe(rigidbody.angularVelocity + Vector3.Scale(Random.onUnitSphere, new Vector3(Random.Range((float) 2f, (float) 4f), Random.Range((float) 0.5f, (float) 1f), Random.Range((float) 2f, (float) 4f))));
            base.ScheduleEvent(new DetachWeaponEvent(rigidbody.velocity, rigidbody.angularVelocity), tank);
        }

        private void SetInertiaTensor(Rigidbody rigidbody, Vector3 size, Vector3 center)
        {
            rigidbody.centerOfMass = center;
            float y = size.y;
            float z = size.z;
            float x = size.x;
            float num4 = y * y;
            float num5 = x * x;
            float num6 = z * z;
            rigidbody.inertiaTensor = new Vector3(num4 + num6, num5 + num6, num4 + num5) * (rigidbody.mass / 12f);
            rigidbody.inertiaTensorRotation = Quaternion.identity;
        }

        public void StopCollide(WeaponDetachColliderComponent colliderComponent)
        {
            colliderComponent.Collider.enabled = false;
            Rigidbody rigidbody = colliderComponent.Rigidbody;
            rigidbody.isKinematic = true;
            rigidbody.detectCollisions = false;
        }

        [OnEventFire]
        public void StopCollide(NodeAddedEvent e, TransparentDetachedWeapon weapon)
        {
            this.StopCollide(weapon.weaponDetachCollider);
        }

        public class DetachedWeaponNode : Node
        {
            public TankGroupComponent tankGroup;
            public WeaponVisualRootComponent weaponVisualRoot;
            public WeaponDetachColliderComponent weaponDetachCollider;
            public DetachedWeaponComponent detachedWeapon;
        }

        public class RemoteTankNode : TankExplosionSystem.TankNode
        {
            public RemoteTankComponent remoteTank;
        }

        public class SelfTankNode : TankExplosionSystem.TankNode
        {
            public SelfComponent self;
        }

        public class TankIncarnationNode : Node
        {
            public TankIncarnationComponent tankIncarnation;
            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node
        {
            public TankComponent tank;
            public TankGroupComponent tankGroup;
            public RigidbodyComponent rigidbody;
            public TankCollidersUnityComponent tankCollidersUnity;
            public TankVisualRootComponent tankVisualRoot;
            public VisualMountPointComponent visualMountPoint;
            public TrackComponent track;
            public AssembledTankComponent assembledTank;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
        }

        public class TransparentDetachedWeapon : Node
        {
            public TankGroupComponent tankGroup;
            public WeaponVisualRootComponent weaponVisualRoot;
            public TransparencyTransitionComponent transparencyTransition;
            public DetachedWeaponComponent detachedWeapon;
            public WeaponDetachColliderComponent weaponDetachCollider;
        }

        public class UnblockedWeaponNode : Node
        {
            public TankGroupComponent tankGroup;
            public WeaponUnblockedComponent weaponUnblocked;
            public WeaponVisualRootComponent weaponVisualRoot;
            public WeaponDetachColliderComponent weaponDetachCollider;
        }
    }
}

