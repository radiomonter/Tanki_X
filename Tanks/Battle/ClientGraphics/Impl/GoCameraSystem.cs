namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class GoCameraSystem : ECSSystem
    {
        [OnEventFire]
        public void InitGoCamera(NodeAddedEvent evt, GoCameraNode goCameraNode, [JoinAll] CameraTargetNode cameraTargetNode)
        {
            int num = Random.Range(0, goCameraNode.goCameraPointsUnity.goCameraPoints.Length);
            goCameraNode.goCamera.goCameraIndex = num;
        }

        [OnEventComplete]
        public void UpdateGoCamera(TimeUpdateEvent evt, GoCameraNode goCameraNode, [JoinAll] CameraTargetNode cameraTargetNode)
        {
            Transform transform = cameraTargetNode.cameraTarget.TargetObject.transform;
            int goCameraIndex = goCameraNode.goCamera.goCameraIndex;
            GoCameraPoint point = goCameraNode.goCameraPointsUnity.goCameraPoints[goCameraIndex];
            Vector3 vector = (Vector3) (transform.rotation * point.poistion);
            TransformData data = new TransformData {
                Position = transform.position + vector,
                Rotation = Quaternion.Euler(transform.rotation.eulerAngles + point.rotation)
            };
            goCameraNode.cameraTransformData.Data = data;
            base.ScheduleEvent(ApplyCameraTransformEvent.ResetApplyCameraTransformEvent(), goCameraNode);
        }

        public class CameraTargetNode : Node
        {
            public CameraTargetComponent cameraTarget;
        }

        public class GoCameraNode : Node
        {
            public GoCameraPointsUnityComponent goCameraPointsUnity;
            public CameraTransformDataComponent cameraTransformData;
            public GoCameraComponent goCamera;
            public BattleCameraComponent battleCamera;
        }
    }
}

