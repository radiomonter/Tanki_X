namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class TrackPoint
    {
        public Vector3 initialPosition;
        public Transform obj;
        public Vector3 objOffset;
        public float desiredVerticalPosition;
        public Vector3 currentPosition;
        public float velocity;

        public TrackPoint(Vector3 position, Transform trackPoint, Vector3 trackPointOffset)
        {
            this.initialPosition = position;
            this.currentPosition = new Vector3(position.x, position.y, position.z);
            this.desiredVerticalPosition = position.y;
            this.obj = trackPoint;
            this.objOffset = trackPointOffset;
        }

        public void UpdateTrackPointPosition(Transform transform, Vector3 position)
        {
            this.obj.localPosition = (transform != this.obj.parent) ? this.obj.parent.InverseTransformPoint(transform.TransformPoint(position + this.objOffset)) : (position + this.objOffset);
        }
    }
}

