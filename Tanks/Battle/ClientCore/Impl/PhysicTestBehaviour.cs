namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using UnityEngine;

    public class PhysicTestBehaviour : MonoBehaviour
    {
        public Rigidbody body1;
        public Rigidbody body2;
        private Vector3 lastPosition1 = Vector3.zero;
        private Vector3 lastPosition2 = Vector3.zero;

        public void FixedUpdate()
        {
            Vector3 position = this.body1.position;
            if (this.lastPosition1 != Vector3.zero)
            {
                this.body1.velocity = (position - this.lastPosition1) / Time.fixedDeltaTime;
            }
            this.lastPosition1 = position;
            BoxCollider component = this.body1.GetComponent<BoxCollider>();
            BoxCollider collider2 = this.body2.GetComponent<BoxCollider>();
            DepenetrationForce.ApplyDepenetrationForce(this.body1, component, this.body2, collider2);
            DepenetrationForce.ApplyDepenetrationForce(this.body2, collider2, this.body1, component);
        }
    }
}

