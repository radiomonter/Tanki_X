namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class TrackController
    {
        private const float additionalAnimationPenetrationBias = 0.02f;
        public TrackPoint[] trackPoints;
        public MovingWheel[] movingWheels;
        public Wheel[] rotatingWheels;
        public TrackSegment[] segments;
        public float centerX;
        private RaycastHit hit;
        private RaycastHit fromHit;
        private RaycastHit toHit;
        public float highDistance;
        public float lowDistance;
        private float slidePosition;

        public TrackController()
        {
            RaycastHit hit = new RaycastHit();
            this.hit = hit;
            RaycastHit hit2 = new RaycastHit();
            this.fromHit = hit2;
            RaycastHit hit3 = new RaycastHit();
            this.toHit = hit3;
        }

        public void Animate(Transform transform, float smoothSpeed, float maxStretchingCoeff)
        {
            this.AnimateTrackPointsPositions(transform);
            this.CorrectTrackSegmentsStretching(maxStretchingCoeff);
            this.UpdateJointsPositions(transform, smoothSpeed);
        }

        private void AnimateTrackPointsPositions(Transform transform)
        {
            float gravityCorrection = this.GetGravityCorrection(transform);
            for (int i = 0; i < this.trackPoints.Length; i++)
            {
                TrackPoint point = this.trackPoints[i];
                Vector3 origin = transform.TransformPoint(GetPointWithVerticalOffset(point.initialPosition, this.highDistance));
                Vector3 vector2 = transform.TransformPoint(GetPointWithVerticalOffset(point.initialPosition, -this.lowDistance * gravityCorrection));
                Vector3 direction = vector2 - origin;
                float magnitude = direction.magnitude;
                point.desiredVerticalPosition = !Physics.Raycast(origin, direction, out this.hit, magnitude, LayerMasks.VISIBLE_FOR_CHASSIS_ANIMATION) ? (point.initialPosition.y - (this.lowDistance * gravityCorrection)) : (point.initialPosition.y + Mathf.Lerp(this.highDistance, -this.lowDistance, this.hit.distance / magnitude));
            }
        }

        private void AnimateTrackPointsWithAdditionalRays(Transform transform)
        {
            for (int i = 0; i < this.segments.Length; i++)
            {
                TrackSegment segment = this.segments[i];
                Vector3 initialPosition = segment.a.initialPosition;
                Vector3 b = segment.b.initialPosition;
                Vector3 origin = transform.TransformPoint(new Vector3(initialPosition.x, segment.a.desiredVerticalPosition + 0.02f, initialPosition.z));
                Vector3 vector4 = transform.TransformPoint(new Vector3(b.x, segment.b.desiredVerticalPosition + 0.02f, b.z));
                Vector3 direction = vector4 - origin;
                float magnitude = direction.magnitude;
                if (Physics.Raycast(origin, direction, out this.fromHit, magnitude, LayerMasks.VISIBLE_FOR_CHASSIS_ANIMATION))
                {
                    Vector3 vector7 = !Physics.Raycast(vector4, -direction, out this.toHit, magnitude, LayerMasks.VISIBLE_FOR_CHASSIS_ANIMATION) ? this.fromHit.point : Vector3.Lerp(this.fromHit.point, this.toHit.point, 0.5f);
                    float t = (vector7 - origin).magnitude / magnitude;
                    Vector3 position = Vector3.Lerp(initialPosition, b, t);
                    position.y = (((1f - t) * initialPosition.y) + (t * b.y)) + this.highDistance;
                    position = transform.TransformPoint(position);
                    direction = vector7 - position;
                    magnitude = direction.magnitude;
                    if (Physics.Raycast(position, direction, out this.hit, magnitude, LayerMasks.VISIBLE_FOR_CHASSIS_ANIMATION))
                    {
                        float num5 = (transform.InverseTransformPoint(this.hit.point).y - ((segment.a.desiredVerticalPosition * (1f - t)) + (segment.b.desiredVerticalPosition * t))) / ((((1f - t) - t) + (t * t)) + (t * t));
                        segment.a.desiredVerticalPosition += (1f - t) * num5;
                        segment.b.desiredVerticalPosition += t * num5;
                    }
                }
            }
        }

        public void AnimateWithAdditionalRays(Transform transform, float smoothSpeed, float maxStretchingCoeff)
        {
            this.AnimateTrackPointsPositions(transform);
            this.AnimateTrackPointsWithAdditionalRays(transform);
            this.CorrectTrackSegmentsStretching(maxStretchingCoeff);
            this.UpdateJointsPositions(transform, smoothSpeed);
        }

        private void CorrectTrackSegmentsStretching(float maxStretchingCoeff)
        {
            for (int i = 0; i < this.segments.Length; i++)
            {
                TrackSegment segment = this.segments[i];
                TrackPoint a = segment.a;
                TrackPoint b = segment.b;
                float num2 = Vector3.Distance(GetPointWithVerticalPosition(a.initialPosition, a.desiredVerticalPosition), GetPointWithVerticalPosition(b.initialPosition, b.desiredVerticalPosition));
                float num3 = num2 / segment.length;
                if (num3 > maxStretchingCoeff)
                {
                    float num6;
                    float num4 = segment.length * maxStretchingCoeff;
                    float num5 = a.initialPosition.x - b.initialPosition.x;
                    float num7 = a.initialPosition.z - b.initialPosition.z;
                    if ((a.desiredVerticalPosition - b.desiredVerticalPosition) < 0f)
                    {
                        num6 = Mathf.Sqrt(((num4 * num4) - (num5 * num5)) - (num7 * num7));
                        a.desiredVerticalPosition = b.desiredVerticalPosition - num6;
                    }
                    else
                    {
                        num6 = Mathf.Sqrt(((num4 * num4) - (num5 * num5)) - (num7 * num7));
                        b.desiredVerticalPosition = a.desiredVerticalPosition - num6;
                    }
                }
            }
        }

        private float GetGravityCorrection(Transform transform)
        {
            Vector3 vector = transform.InverseTransformDirection(Vector3.up);
            return Mathf.Clamp01((vector.y / vector.magnitude) / 0.3f);
        }

        private static Vector3 GetPointWithVerticalOffset(Vector3 initialPoint, float verticalOffset) => 
            new Vector3(initialPoint.x, initialPoint.y + verticalOffset, initialPoint.z);

        private static Vector3 GetPointWithVerticalPosition(Vector3 point, float verticalPosition) => 
            new Vector3(point.x, verticalPosition, point.z);

        private unsafe void UpdateJointsPositions(Transform transform, float smoothSpeed)
        {
            float num = Time.deltaTime * smoothSpeed;
            for (int i = 0; i < this.trackPoints.Length; i++)
            {
                TrackPoint point = this.trackPoints[i];
                float num3 = point.desiredVerticalPosition - point.currentPosition.y;
                num3 = (num3 <= 0f) ? ((-num3 <= num) ? num3 : -num) : ((num3 <= num) ? num3 : num);
                Vector3* vectorPtr1 = &point.currentPosition;
                vectorPtr1->y += num3;
            }
            for (int j = 0; j < this.trackPoints.Length; j++)
            {
                TrackPoint point2 = this.trackPoints[j];
                point2.UpdateTrackPointPosition(transform, point2.currentPosition);
            }
            for (int k = 0; k < this.movingWheels.Length; k++)
            {
                MovingWheel wheel = this.movingWheels[k];
                wheel.UpdateObjPosition(transform, wheel.nearestPoint.currentPosition);
            }
        }

        public void UpdateWheelsRotation(float offset)
        {
            this.slidePosition = offset;
            for (int i = 0; i < this.rotatingWheels.Length; i++)
            {
                Wheel wheel = this.rotatingWheels[i];
                if (wheel.radius > 0f)
                {
                    wheel.SetRotation((this.slidePosition / wheel.radius) / 0.01745329f);
                }
            }
            for (int j = 0; j < this.movingWheels.Length; j++)
            {
                Wheel wheel2 = this.movingWheels[j];
                if (wheel2.radius > 0f)
                {
                    wheel2.SetRotation((this.slidePosition / wheel2.radius) / 0.01745329f);
                }
            }
        }
    }
}

