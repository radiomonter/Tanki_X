namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CameraSaveData
    {
        private CameraSaveData()
        {
        }

        public static CameraSaveData CreateFollowData(string userUid, float followCameraBezierPositionRatio, float followCameraBezierPositionRatioOffset) => 
            new CameraSaveData { 
                UserUid = userUid,
                Type = CameraType.Follow,
                FollowCameraBezierPositionRatio = followCameraBezierPositionRatio,
                FollowCameraBezierPositionRatioOffset = followCameraBezierPositionRatioOffset
            };

        public static CameraSaveData CreateFreeData(Transform transform)
        {
            CameraSaveData data = new CameraSaveData {
                Type = CameraType.Free
            };
            Tanks.Battle.ClientGraphics.Impl.TransformData data2 = new Tanks.Battle.ClientGraphics.Impl.TransformData {
                Position = transform.position,
                Rotation = transform.rotation
            };
            data.TransformData = data2;
            return data;
        }

        public static CameraSaveData CreateMouseOrbitData(string userUid, float mouseOrbitDistance, Quaternion mouseOrbitTargetRotation) => 
            new CameraSaveData { 
                UserUid = userUid,
                Type = CameraType.MouseOrbit,
                MouseOrbitDistance = mouseOrbitDistance,
                MouseOrbitTargetRotation = mouseOrbitTargetRotation
            };

        public string UserUid { get; private set; }

        public CameraType Type { get; private set; }

        public Tanks.Battle.ClientGraphics.Impl.TransformData TransformData { get; private set; }

        public float FollowCameraBezierPositionRatio { get; private set; }

        public float FollowCameraBezierPositionRatioOffset { get; private set; }

        public float MouseOrbitDistance { get; private set; }

        public Quaternion MouseOrbitTargetRotation { get; private set; }
    }
}

