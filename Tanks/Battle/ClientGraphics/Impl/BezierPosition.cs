namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class BezierPosition
    {
        private const float MAX_OFFSET = 0.1f;
        private float baseRatio = 0.05f;
        private float offset;
        public Vector2 cameraPosition;
        public Vector2 point0;
        public Vector2 point1;
        public Vector2 point2;
        public Vector2 point3;
        public float elevationAngle;
        public float distanceToPivot;

        public BezierPosition()
        {
            Vector2 vector = new Vector2();
            this.cameraPosition = vector;
            this.point0 = new Vector2(1.45f, 5.45f);
            this.point1 = new Vector2(9.3f, 13.95f);
            this.point2 = new Vector2(22.45f, 15.65f);
            this.point3 = new Vector2(31.05f, 7.6f);
            this.Apply();
        }

        public void Apply()
        {
            this.cameraPosition = Bezier.PointOnCurve(this.ratio, this.point0, this.point1, this.point2, this.point3);
            this.elevationAngle = Mathf.Atan2(this.cameraPosition.x, this.cameraPosition.y);
            this.distanceToPivot = this.cameraPosition.magnitude;
        }

        public float GetBaseRatio() => 
            this.baseRatio;

        public float GetCameraHeight() => 
            this.cameraPosition.x;

        public float GetCameraHorizontalDistance() => 
            this.cameraPosition.y;

        public float GetDistanceToPivot() => 
            this.distanceToPivot;

        public float GetRatioOffset() => 
            (this.offset + 0.05f) / 0.1f;

        public void SetBaseRatio(float value)
        {
            this.baseRatio = Mathf.Clamp01(value);
            this.Apply();
        }

        public void SetRatioOffset(float value)
        {
            this.offset = Mathf.Lerp(-0.05f, 0.05f, Mathf.Clamp01(value));
            this.Apply();
        }

        private float ratio =>
            Mathf.Clamp01(this.baseRatio + this.offset);
    }
}

