namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class DroneMovementSystem : ECSSystem
    {
        private const float droneLookDownAngle = 0.5f;

        private void ApplyMovingForce(Rigidbody body, DroneMoveConfigComponent config, DroneOwnerComponent owner)
        {
            Vector3 vector3 = (owner.Rigidbody.transform.position + config.FlyPosition) - body.position;
            float magnitude = vector3.magnitude;
            Vector3 vector4 = vector3 / magnitude;
            if (magnitude > 1f)
            {
                body.AddForceSafe(vector4 * Mathf.Clamp(magnitude * config.Acceleration, 0f, config.MoveSpeed));
            }
        }

        public void ApplyStabilization(Rigidbody body)
        {
            float num2 = 5f * body.mass;
            Vector3 vector = new Vector3(0f, 0f, 1f);
            if (!this.ApplyWingForce(body, body.transform.right, vector.normalized * num2))
            {
                Vector3 vector2 = new Vector3(0f, 0f, -1f);
                this.ApplyWingForce(body, -body.transform.right, vector2.normalized * num2);
            }
            float num3 = 3f * body.mass;
            Vector3 vector3 = new Vector3(-1f, 0f, 0f);
            if (!this.ApplyWingForce(body, body.transform.forward, vector3.normalized * num3))
            {
                Vector3 vector4 = new Vector3(1f, 0f, 0f);
                this.ApplyWingForce(body, -body.transform.forward, vector4.normalized * num3);
            }
        }

        private void ApplyTargetingForce(Rigidbody body, Vector3 targetingDirection)
        {
            float num = Vector3.Dot(targetingDirection, body.transform.forward);
            float f = Vector3.Dot(targetingDirection, body.transform.right);
            body.AddTorqueSafe(0f, ((Mathf.Sign(f) * 4f) * body.mass) * (1.5f - num), 0f);
            float x = Mathf.Clamp((float) (((Mathf.Sign(Vector3.Dot(targetingDirection, -body.transform.up)) * 4f) * body.mass) * (1.5f - num)), (float) -2f, (float) 4f);
            body.AddRelativeTorque(x, 0f, 0f);
        }

        public bool ApplyWingForce(Rigidbody body, Vector3 wingDirection, Vector3 strength)
        {
            float num = -Vector3.Dot(wingDirection, Vector3.up);
            if (num <= 0f)
            {
                return false;
            }
            body.AddRelativeTorqueSafe(strength * num);
            return true;
        }

        [OnEventFire]
        public unsafe void DroneMovement(FixedUpdateEvent e, DroneNode drone)
        {
            DroneOwnerComponent droneOwner = drone.droneOwner;
            Rigidbody body = drone.rigidbody.Rigidbody;
            DroneMoveConfigComponent droneMoveConfig = drone.droneMoveConfig;
            if (droneOwner.Incarnation.Alive && droneOwner.Rigidbody)
            {
                this.ApplyMovingForce(body, droneMoveConfig, droneOwner);
            }
            Rigidbody rigidbody = drone.rigidbody.Rigidbody;
            this.ApplyStabilization(rigidbody);
            Rigidbody rigidbody3 = null;
            if (drone.Entity.HasComponent<UnitTargetComponent>())
            {
                Entity target = drone.Entity.GetComponent<UnitTargetComponent>().Target;
                if (target.HasComponent<RigidbodyComponent>())
                {
                    rigidbody3 = target.GetComponent<RigidbodyComponent>().Rigidbody;
                }
            }
            if (rigidbody3 != null)
            {
                Vector3 position = rigidbody3.position;
                Vector3* vectorPtr1 = &position;
                vectorPtr1->y += 0.5f;
                this.ApplyTargetingForce(rigidbody, (position - rigidbody.position).normalized);
            }
            else if (droneOwner.Incarnation.Alive && droneOwner.Rigidbody)
            {
                this.ApplyTargetingForce(rigidbody, (droneOwner.Rigidbody.transform.forward - (Vector3.up * 0.5f)).normalized);
            }
        }

        public class DroneNode : Node
        {
            public DroneEffectComponent droneEffect;
            public DroneOwnerComponent droneOwner;
            public UnitMoveComponent unitMove;
            public UnitGroupComponent unitGroup;
            public RigidbodyComponent rigidbody;
            public DroneMoveConfigComponent droneMoveConfig;
        }
    }
}

