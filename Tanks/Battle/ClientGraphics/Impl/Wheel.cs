namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class Wheel
    {
        public float radius;
        public Transform obj;
        private float rotation;

        public Wheel(Transform obj)
        {
            this.obj = obj;
        }

        public void SetRotation(float angle)
        {
            this.obj.localRotation *= Quaternion.AngleAxis(angle - this.rotation, Vector3.right);
            this.rotation = angle;
        }
    }
}

