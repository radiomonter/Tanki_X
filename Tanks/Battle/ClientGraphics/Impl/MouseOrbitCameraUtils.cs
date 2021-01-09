namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public static class MouseOrbitCameraUtils
    {
        public static unsafe Vector3 GetClippedPosition(Vector3 centralPosition, Vector3 targetPosition, float maxDistance)
        {
            RaycastHit hit;
            if (Physics.Linecast(centralPosition, targetPosition, out hit, LayerMasks.STATIC))
            {
                targetPosition = hit.point;
            }
            targetPosition -= (targetPosition - centralPosition).normalized * MouseOrbitCameraConstants.CLIP_DISTANCE;
            if (Physics.Linecast(targetPosition, targetPosition + (Vector3.down * MouseOrbitCameraConstants.CLIP_DISTANCE), out hit, LayerMasks.STATIC))
            {
                targetPosition.y = hit.point.y + MouseOrbitCameraConstants.CLIP_DISTANCE;
            }
            Vector2 vector3 = new Vector2(targetPosition.x - centralPosition.x, targetPosition.z - centralPosition.z);
            float num = 1f - (vector3.magnitude / maxDistance);
            Vector3* vectorPtr1 = &targetPosition;
            vectorPtr1->y += num * MouseOrbitCameraConstants.PROXIMITY_ELEVATION;
            return targetPosition;
        }

        public static TransformData GetTargetMouseOrbitCameraTransformData(Transform cameraTargetTransform, float mouseOrbitCameraDistance, Quaternion mouseOrbitCameraTargetRotation)
        {
            TransformData data = new TransformData {
                Rotation = Quaternion.Euler(NormalizeEuler(mouseOrbitCameraTargetRotation.eulerAngles))
            };
            Vector3 position = cameraTargetTransform.position;
            data.Position = GetClippedPosition(position, position + (data.Rotation * new Vector3(0f, 0f, -mouseOrbitCameraDistance)), MouseOrbitCameraConstants.MAX_DISTANCE);
            return data;
        }

        public static Vector3 NormalizeEuler(Vector3 euler) => 
            NormalizeEuler(euler, MouseOrbitCameraConstants.X_MIN_ANGLE, MouseOrbitCameraConstants.X_MAX_ANGLE);

        private static Vector3 NormalizeEuler(Vector3 euler, float xMinAngle, float xMaxAngle)
        {
            euler.x = Mathf.Clamp(((euler.x + 180f) % 360f) - 180f, xMinAngle, xMaxAngle);
            euler.y = (euler.y + 360f) % 360f;
            euler.z = 0f;
            return euler;
        }
    }
}

