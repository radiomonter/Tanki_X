namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public static class CameraPositionCalculator
    {
        private const float minDistanceToTarget = 3f;
        private const float sphereRadius = 0.5f;
        private const float MIN_CAMERA_ANGLE = 0.08726647f;
        private const float FIXED_PITCH = 0.1745329f;
        private const float PITCH_CORRECTION_COEFF = 1f;
        private const float MAX_POSITION_ERROR = 0.1f;
        private const float MAX_ANGLE_ERROR = 0.01745329f;
        private const float LINEAR_SPEED_THRESHOLD = 0.1f;
        private const float ANGULAR_SPEED_THRESHOLD = 0.1f;
        private const float CORRECTION_HEIGHT = 0.2f;
        private const int MAX_CAMERA_MOVE_SPEED = 5;
        private const float MAX_CAMERA_ROTATE_SPEED = 9f;
        private const float MIN_CAMERA_ROTATE_SPEED = 3f;
        private const float MAX_ANGLE_BETWEEN_CAMERA_AND_WEAPON = 90f;
        private const float MIN_TANK_LENGTH = 1.2f;
        private const float MAX_TANK_LENGTH = 3.181962f;

        private static Vector3 CalculateCameraDirection(float cameraElevationAngleTan, Vector3 targetDirection)
        {
            Vector3 horizontalDirection = GetHorizontalDirection(targetDirection);
            Vector3 vector2 = new Vector3(0f, -cameraElevationAngleTan, 0f);
            return (horizontalDirection + vector2).normalized;
        }

        public static Vector3 CalculateCameraPosition(Transform target, BaseRendererComponent tankRendererComponent, Vector3 tankBoundsCenter, BezierPosition bezierPosition, CameraData cameraData, Vector3 cameraOffset, float additionalAngle)
        {
            GameObject gameObject = target.gameObject;
            Vector3 position = target.position;
            Vector3 localPosition = target.parent.localPosition;
            float z = tankRendererComponent.Mesh.bounds.extents.z;
            float num3 = z * Mathf.Max((float) 0f, (float) ((z - 1.2f) / 1.981962f));
            if (target.parent != null)
            {
                Vector3 vector4 = target.parent.InverseTransformPoint(tankBoundsCenter);
                num3 -= vector4.z - localPosition.z;
            }
            Vector3 vector5 = new Vector3(0f, 0f, -num3);
            Quaternion quaternion = Quaternion.Euler(target.rotation.eulerAngles + new Vector3(0f, additionalAngle, 0f));
            Vector3 vector6 = (Vector3) (quaternion * (vector5 - new Vector3(0f, 0f, localPosition.z)));
            Vector3 targetPosition = (position + (quaternion * (localPosition + cameraOffset))) + (Vector3.up * 0.2f);
            bool hasCollision = false;
            bool flag2 = false;
            Vector3 vector11 = CalculateCollisionPoint(targetPosition, CalculateCameraDirection(bezierPosition.GetCameraHeight() / bezierPosition.GetCameraHorizontalDistance(), (Vector3) (Quaternion.Euler(new Vector3(0f, additionalAngle, 0f)) * target.forward)), bezierPosition.GetDistanceToPivot(), gameObject, out hasCollision);
            float magnitude = (targetPosition - vector11).magnitude;
            cameraData.collisionDistanceRatio = magnitude / bezierPosition.GetDistanceToPivot();
            if (magnitude < 3f)
            {
                float rayLength = 3f - magnitude;
                vector11 = CalculateCollisionPoint(vector11, -Vector3.up, rayLength, gameObject, out flag2);
            }
            else
            {
                RaycastHit hit;
                int layerMask = LayerMasksUtils.RemoveLayerFromMask(LayerMasks.STATIC, gameObject.layer);
                Vector3 normalized = vector6.normalized;
                vector11 = !Physics.Raycast(vector11, normalized, out hit, vector6.magnitude, layerMask) ? (vector11 + vector6) : (vector11 + (Mathf.Max((float) (hit.distance - 0.5f), (float) 0f) * normalized));
            }
            return vector11;
        }

        private static Vector3 CalculateCollisionPoint(Vector3 targetPosition, Vector3 cameraDirection, float rayLength, GameObject exlusionGameObject, out bool hasCollision)
        {
            RaycastHit hit;
            Vector3 vector = new Vector3();
            int layerMask = LayerMasksUtils.RemoveLayerFromMask(LayerMasks.STATIC, exlusionGameObject.layer);
            hasCollision = Physics.Raycast(targetPosition, -cameraDirection, out hit, rayLength, layerMask);
            return (!hasCollision ? (targetPosition - (cameraDirection * rayLength)) : (hit.point + (0.5f * hit.normal)));
        }

        public static Vector3 CalculateLinearMovement(float dt, Vector3 cameraCalculated, Vector3 cameraReal, CameraData cameraData, Transform target, bool mouse)
        {
            Vector3 vector = cameraCalculated - cameraReal;
            float magnitude = vector.magnitude;
            if (magnitude > 0.1f)
            {
                cameraData.linearSpeed = 5f * (magnitude - 0.1f);
            }
            if (mouse)
            {
                float b = magnitude / dt;
                cameraData.linearSpeed = Mathf.Lerp(cameraData.linearSpeed, b, Mathf.Sin((270f + Mathf.Lerp(0f, 90f, Vector3.Angle(cameraCalculated - target.position, cameraReal - target.position) / 90f)) * 0.01745329f) + 1f);
            }
            vector.Normalize();
            vector *= Mathf.Clamp(cameraData.linearSpeed * dt, 0f, magnitude);
            cameraReal += vector;
            cameraData.linearSpeed = MathUtil.Snap(cameraData.linearSpeed, 0f, 0.1f);
            return cameraReal;
        }

        public static void CalculatePitchMovement(ref Vector3 rotation, BezierPosition bezierPosition, float dt, CameraData cameraData, bool mouse = false)
        {
            float angleError = MathUtil.ClampAngleFast(-GetPitchAngle(cameraData, bezierPosition) - MathUtil.ClampAngle(rotation.x));
            cameraData.pitchSpeed = GetAngularSpeed(angleError, cameraData.pitchSpeed, mouse);
            float num4 = cameraData.pitchSpeed * dt;
            if (((angleError > 0f) && (num4 > angleError)) || ((angleError < 0f) && (num4 < angleError)))
            {
                num4 = angleError;
            }
            rotation.x += num4;
            cameraData.pitchSpeed = MathUtil.Snap(cameraData.pitchSpeed, 0f, 0.1f);
        }

        public static void CalculateYawMovement(Vector3 targetDirection, ref Vector3 rotation, float dt, CameraData cameraData, bool mouse = false)
        {
            float angleError = MathUtil.ClampAngleFast(Mathf.Atan2(targetDirection.x, targetDirection.z) - MathUtil.ClampAngle(rotation.y));
            cameraData.yawSpeed = GetAngularSpeed(angleError, cameraData.yawSpeed, mouse);
            float num4 = cameraData.yawSpeed * dt;
            if (((angleError > 0f) && (num4 > angleError)) || ((angleError < 0f) && (num4 < angleError)))
            {
                num4 = angleError;
            }
            rotation.y += num4;
            cameraData.yawSpeed = MathUtil.Snap(cameraData.yawSpeed, 0f, 0.1f);
        }

        private static float GetAngularSpeed(float angleError, float currentSpeed, bool mouse = false)
        {
            float num = !mouse ? 3f : 9f;
            return ((angleError >= -0.01745329f) ? ((angleError <= 0.01745329f) ? currentSpeed : (num * (angleError - 0.01745329f))) : (num * (angleError + 0.01745329f)));
        }

        private static Vector3 GetHorizontalDirection(Vector3 targetDirection)
        {
            float num = Mathf.Sqrt((targetDirection.x * targetDirection.x) + (targetDirection.z * targetDirection.z));
            Vector3 vector = new Vector3();
            if (num < 1E-05f)
            {
                vector.x = 1f;
                vector.z = 0f;
            }
            else
            {
                vector.x = targetDirection.x / num;
                vector.z = targetDirection.z / num;
            }
            vector.y = 0f;
            return vector;
        }

        public static float GetPitchAngle(CameraData cameraData, BezierPosition bezierPosition)
        {
            float f = bezierPosition.elevationAngle - 0.1745329f;
            if (f < 0f)
            {
                f = 0f;
            }
            float collisionDistanceRatio = cameraData.collisionDistanceRatio;
            if ((collisionDistanceRatio >= 1f) || ((f < 0.08726647f) || !cameraData.pitchCorrectionEnabled))
            {
                return -f;
            }
            float num3 = bezierPosition.GetDistanceToPivot() * Mathf.Sin(f);
            return -Mathf.Atan2(collisionDistanceRatio * num3, (1f * num3) * ((1f / Mathf.Tan(f)) - ((1f - collisionDistanceRatio) / Mathf.Tan(bezierPosition.elevationAngle))));
        }

        public static TransformData GetTargetFollowCameraTransformData(Transform target, BaseRendererComponent tankRendererComponent, Vector3 tankBoundsCenter, BezierPosition bezierPosition, Vector3 cameraOffset)
        {
            CameraData cameraData = new CameraData();
            float cameraHeight = bezierPosition.GetCameraHeight();
            Vector3 vector2 = target.TransformDirection(Vector3.forward);
            Vector3 vector3 = new Vector3(-GetPitchAngle(cameraData, bezierPosition), Mathf.Atan2(vector2.x, vector2.z), 0f);
            return new TransformData { 
                Position = CalculateCameraPosition(target, tankRendererComponent, tankBoundsCenter, bezierPosition, cameraData, cameraOffset, 0f),
                Rotation = Quaternion.Euler(vector3 * 57.29578f)
            };
        }

        public static void SmoothReturnRoll(ref Vector3 rotation, float rollReturnSpeedDegPerSec, float deltaTime)
        {
            float num = rollReturnSpeedDegPerSec * 0.01745329f;
            rotation.z = MathUtil.ClampAngle(rotation.z);
            if (rotation.z > 0f)
            {
                rotation.z -= num * deltaTime;
                rotation.z = SnapNegativeRotationToZero(rotation.z);
            }
            else if (rotation.z < 0f)
            {
                rotation.z += num * deltaTime;
                rotation.z = SnapPositiveRotationToZero(rotation.z);
            }
        }

        private static float SnapNegativeRotationToZero(float rotationAngle) => 
            (rotationAngle >= 0f) ? rotationAngle : 0f;

        private static float SnapPositiveRotationToZero(float rotationAngle) => 
            (rotationAngle <= 0f) ? rotationAngle : 0f;
    }
}

