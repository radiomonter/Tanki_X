namespace Tanks.Battle.ClientCore.API
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using UnityEngine;

    public class JumpTrigger : ECSBehaviour
    {
        public Transform targetPoint;
        public float angle = 45f;
        private float sqrActivateDistance = 4f;

        private Vector3 CalculateJumpVelocity(Vector3 startPosition, Vector3 targetPosition)
        {
            targetPosition.y = startPosition.y;
            Vector3 vector = targetPosition - startPosition;
            float magnitude = vector.magnitude;
            float f = this.angle * 0.01745329f;
            vector.Normalize();
            vector += Vector3.up * Mathf.Tan(f);
            vector.Normalize();
            return (vector * Mathf.Sqrt((Physics.gravity.magnitude * magnitude) / Mathf.Sin(2f * f)));
        }

        private void OnTriggerStay(Collider other)
        {
            Rigidbody attachedRigidbody = other.attachedRigidbody;
            if ((attachedRigidbody.position - base.transform.position).sqrMagnitude <= this.sqrActivateDistance)
            {
                TargetBehaviour componentInParent = attachedRigidbody.GetComponentInParent<TargetBehaviour>();
                if (componentInParent && componentInParent.TargetEntity.HasComponent<TankSyncComponent>())
                {
                    Vector3 velocity = this.CalculateJumpVelocity(attachedRigidbody.position, this.targetPoint.position);
                    if (componentInParent.TargetEntity.HasComponent<TankJumpComponent>())
                    {
                        componentInParent.TargetEntity.GetComponent<TankJumpComponent>().StartJump(velocity);
                    }
                    else
                    {
                        TankJumpComponent component = new TankJumpComponent();
                        component.StartJump(velocity);
                        componentInParent.TargetEntity.AddComponent(component);
                    }
                }
            }
        }
    }
}

