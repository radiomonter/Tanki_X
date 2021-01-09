namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class MovingWheel : Wheel
    {
        public TrackPoint nearestPoint;
        public Vector3 objOffset;

        public MovingWheel(Transform obj) : base(obj)
        {
        }

        public MovingWheel(Transform obj, TrackPoint nearestPoint, Vector3 offset) : base(obj)
        {
            this.nearestPoint = nearestPoint;
            this.objOffset = offset;
        }

        public void UpdateObjPosition(Transform transform, Vector3 position)
        {
            base.obj.localPosition = (transform != base.obj.parent) ? base.obj.parent.InverseTransformPoint(transform.TransformPoint(position + this.objOffset)) : (position + this.objOffset);
        }
    }
}

