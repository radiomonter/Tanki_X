namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class UnitMoveSmootherComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private float smoothingSpeed = 5f;
        private Vector3 smoothPositionDelta;
        private Quaternion smoothRotationDelta;
        private Vector3 lastPosition;
        private Quaternion lastRotation;
        private Rigidbody body;

        public void AfterSetMovement()
        {
            base.transform.position = this.lastPosition;
            base.transform.rotation = this.lastRotation;
            this.smoothPositionDelta = base.transform.localPosition;
            this.smoothRotationDelta = base.transform.localRotation;
            this.LateUpdate();
        }

        public void BeforeSetMovement()
        {
            this.lastPosition = base.transform.position;
            this.lastRotation = base.transform.rotation;
        }

        private void LateUpdate()
        {
            if (this.body)
            {
                this.smoothPositionDelta = Vector3.Lerp(this.smoothPositionDelta, Vector3.zero, this.smoothingSpeed * Time.deltaTime);
                this.smoothRotationDelta = Quaternion.Slerp(this.smoothRotationDelta, Quaternion.identity, this.smoothingSpeed * Time.smoothDeltaTime);
                base.transform.SetLocalPositionSafe(this.smoothPositionDelta);
                base.transform.SetLocalRotationSafe(this.smoothRotationDelta);
                this.body.centerOfMass = this.smoothPositionDelta;
            }
        }

        private void Start()
        {
            this.body = base.GetComponentInParent<Rigidbody>();
        }
    }
}

