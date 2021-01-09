namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class TankFallingSystem : ECSSystem
    {
        private const float FLAT_FALLING_ANGLE = 30f;
        private const float VERTICAL_FALLING_ANGLE = 70f;
        private const float DEBUG_RAY_DURATION = 0.5f;
        private const float NRM_LENGTH = 2f;

        private void ApplyFall(Entity tankEntity, Vector3 previousVelocity, TankFallingComponent tankFalling, TrackComponent track, ChassisConfigComponent chassisConfig, TankCollisionComponent tankCollision, RigidbodyComponent rigidbody, bool fallingByTrack)
        {
            if ((tankFalling != null) && (!tankFalling.IsGrounded && rigidbody.Rigidbody))
            {
                bool flag;
                Vector3 planeNormal = this.GetFallingNrm(fallingByTrack, track, chassisConfig, tankCollision, out flag);
                Vector3 vector2 = Vector3.ProjectOnPlane(previousVelocity, planeNormal);
                Vector3 position = rigidbody.Rigidbody.transform.position;
                TankFallingType type = this.DefineFallingCollisionMode(flag, fallingByTrack, planeNormal);
                TankFallEvent eventInstance = new TankFallEvent {
                    FallingPower = (previousVelocity - vector2).sqrMagnitude,
                    FallingType = type,
                    Velocity = previousVelocity
                };
                if ((type == TankFallingType.SLOPED_STATIC_WITH_COLLISION) && this.CheckTankCollisionNotNull(tankCollision))
                {
                    eventInstance.FallingTransform = tankCollision.Collision.transform;
                }
                base.ScheduleEvent(eventInstance, tankEntity);
                tankFalling.IsGrounded = true;
            }
        }

        private void ApplyTrackRayNormal(ref Vector3 currentNrm, SuspensionRay ray)
        {
            if (ray.hasCollision)
            {
                currentNrm += ray.rayHit.normal;
            }
        }

        private bool CheckTankCollisionNotNull(TankCollisionComponent tankCollision) => 
            (tankCollision.HasCollision && (tankCollision.Collision != null)) && (tankCollision.Collision.rigidbody != null);

        private bool CheckTankLayer(int layer) => 
            layer == Layers.TANK_TO_TANK;

        private TankFallingType DefineFallingCollisionMode(bool isFallingOnTank, bool isFallingByTracks, Vector3 midNrm)
        {
            if (isFallingOnTank)
            {
                return TankFallingType.TANK;
            }
            float num = Mathf.Abs(Vector3.Angle(midNrm, Vector3.up));
            return ((num > 30f) ? ((num < 70f) ? (!isFallingByTracks ? TankFallingType.SLOPED_STATIC_WITH_COLLISION : TankFallingType.SLOPED_STATIC_WITH_TRACKS) : TankFallingType.VERTICAL_STATIC) : TankFallingType.FLAT_STATIC);
        }

        [OnEventFire]
        public void DisableTankFalling(NodeRemoveEvent evt, ActivatedTankNode tank)
        {
            tank.Entity.RemoveComponentIfPresent<TankFallingComponent>();
        }

        private int GetCollisionContacts(TankCollisionComponent tankCollision)
        {
            Collision collision = tankCollision.Collision;
            return ((collision != null) ? ((collision.contacts != null) ? collision.contacts.Length : 0) : 0);
        }

        private Vector3 GetFallingNrm(bool fallingByTrack, TrackComponent track, ChassisConfigComponent chassisConfig, TankCollisionComponent tankCollision, out bool isFallingOnTank)
        {
            Vector3 zero = Vector3.zero;
            isFallingOnTank = false;
            if (!fallingByTrack)
            {
                Collision collision = tankCollision.Collision;
                if (!this.CheckTankCollisionNotNull(tankCollision))
                {
                    return zero.normalized;
                }
                ContactPoint[] contacts = collision.contacts;
                if (contacts == null)
                {
                    return zero.normalized;
                }
                int length = contacts.Length;
                for (int i = 0; i < length; i++)
                {
                    ContactPoint point = contacts[i];
                    zero += point.normal;
                    if (!isFallingOnTank)
                    {
                        Collider otherCollider = point.otherCollider;
                        if (this.ValidateCollider(otherCollider))
                        {
                            int layer = otherCollider.gameObject.layer;
                            isFallingOnTank |= this.CheckTankLayer(layer);
                        }
                    }
                }
            }
            else
            {
                int numRaysPerTrack = chassisConfig.NumRaysPerTrack;
                SuspensionRay[] rays = track.LeftTrack.rays;
                SuspensionRay[] rayArray2 = track.RightTrack.rays;
                for (int i = 0; i < numRaysPerTrack; i++)
                {
                    SuspensionRay ray = rays[i];
                    SuspensionRay ray2 = rayArray2[i];
                    this.ApplyTrackRayNormal(ref zero, ray);
                    this.ApplyTrackRayNormal(ref zero, ray2);
                    if (!isFallingOnTank)
                    {
                        if (ray.hasCollision)
                        {
                            if ((ray.rayHit.collider == null) || (ray.rayHit.collider.gameObject == null))
                            {
                                return zero.normalized;
                            }
                            if (this.ValidateCollider(ray.rayHit.collider))
                            {
                                int layer = ray.rayHit.collider.gameObject.layer;
                                isFallingOnTank |= this.CheckTankLayer(layer);
                            }
                        }
                        if (ray2.hasCollision)
                        {
                            if ((ray2.rayHit.collider == null) || (ray2.rayHit.collider.gameObject == null))
                            {
                                return zero.normalized;
                            }
                            if (this.ValidateCollider(ray2.rayHit.collider))
                            {
                                int layer = ray2.rayHit.collider.gameObject.layer;
                                isFallingOnTank |= this.CheckTankLayer(layer);
                            }
                        }
                    }
                }
            }
            return zero.normalized;
        }

        private int GetTrackContacts(TrackComponent track) => 
            track.LeftTrack.numContacts + track.RightTrack.numContacts;

        [OnEventFire]
        public void InitTankFalling(NodeAddedEvent evt, ActivatedTankNode tank)
        {
            Entity entity = tank.Entity;
            TankFallingComponent component = new TankFallingComponent();
            TrackComponent track = tank.track;
            TankCollisionComponent tankCollision = tank.tankCollision;
            component.PreviousCollisionContactsCount = this.GetCollisionContacts(tankCollision);
            component.PreviousTrackContactsCount = this.GetTrackContacts(track);
            component.IsGrounded = true;
            component.PreviousVelocity = Vector3.zero;
            entity.AddComponent(component);
        }

        private void UpdateGroundedStatus(TankFallingComponent tankFalling, int deltaTrackContacts, int currentCollisionContactsCount, int currentTrackContactsCount)
        {
            if (tankFalling.IsGrounded)
            {
                if (deltaTrackContacts < 0)
                {
                    tankFalling.IsGrounded = false;
                }
                else if ((currentCollisionContactsCount == 0) && (currentTrackContactsCount == 0))
                {
                    tankFalling.IsGrounded = false;
                }
            }
        }

        [OnEventComplete]
        public void UpdateTankFalling(FixedUpdateEvent evt, FallingTankNode tank)
        {
            TankFallingComponent tankFalling = tank.tankFalling;
            TrackComponent track = tank.track;
            RigidbodyComponent rigidbody = tank.rigidbody;
            TankCollisionComponent tankCollision = tank.tankCollision;
            ChassisConfigComponent chassisConfig = tank.chassisConfig;
            Entity tankEntity = tank.Entity;
            int trackContacts = this.GetTrackContacts(track);
            int collisionContacts = this.GetCollisionContacts(tankCollision);
            int num5 = collisionContacts - tankFalling.PreviousCollisionContactsCount;
            int deltaTrackContacts = trackContacts - tankFalling.PreviousTrackContactsCount;
            Vector3 previousVelocity = tankFalling.PreviousVelocity;
            tankFalling.PreviousCollisionContactsCount = collisionContacts;
            tankFalling.PreviousTrackContactsCount = trackContacts;
            tankFalling.PreviousVelocity = rigidbody.Rigidbody.velocity;
            if (deltaTrackContacts > 0)
            {
                this.ApplyFall(tankEntity, previousVelocity, tankFalling, track, chassisConfig, tankCollision, rigidbody, true);
            }
            else if ((num5 > 0) && (trackContacts == 0))
            {
                this.ApplyFall(tankEntity, previousVelocity, tankFalling, track, chassisConfig, tankCollision, rigidbody, false);
            }
            else
            {
                this.UpdateGroundedStatus(tankFalling, deltaTrackContacts, collisionContacts, trackContacts);
            }
        }

        private bool ValidateCollider(Collider collider) => 
            (collider != null) ? (collider.gameObject != null) : false;

        public class ActivatedTankNode : Node
        {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankMovableComponent tankMovable;
            public TrackComponent track;
            public TankCollisionComponent tankCollision;
            public RigidbodyComponent rigidbody;
            public ChassisConfigComponent chassisConfig;
        }

        public class FallingTankNode : TankFallingSystem.ActivatedTankNode
        {
            public TankFallingComponent tankFalling;
        }
    }
}

