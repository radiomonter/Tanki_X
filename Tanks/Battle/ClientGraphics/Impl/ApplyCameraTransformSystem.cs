namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class ApplyCameraTransformSystem : ECSSystem
    {
        [OnEventFire]
        public void ApplyCameraTransform(ApplyCameraTransformEvent e, BattleCameraNode battleCamera)
        {
            CameraComponent camera = battleCamera.camera;
            CameraTransformDataComponent cameraTransformData = battleCamera.cameraTransformData;
            Transform root = battleCamera.cameraRootTransform.Root;
            Vector3 position = cameraTransformData.Data.Position;
            Quaternion rotation = cameraTransformData.Data.Rotation;
            float t = 1f;
            float num2 = 1f;
            if (e.DeltaTimeValid)
            {
                float deltaTime = e.DeltaTime;
                t = !e.PositionSmoothingRatioValid ? battleCamera.applyCameraTransform.positionSmoothingRatio : e.PositionSmoothingRatio;
                num2 = !e.RotationSmoothingRatioValid ? battleCamera.applyCameraTransform.rotationSmoothingRatio : e.RotationSmoothingRatio;
                battleCamera.applyCameraTransform.positionSmoothingRatio = t;
                battleCamera.applyCameraTransform.rotationSmoothingRatio = num2;
                t *= deltaTime;
                num2 *= deltaTime;
            }
            root.SetPositionSafe(Vector3.Lerp(root.position, position, t));
            Vector3 eulerAngles = rotation.eulerAngles;
            Vector3 vector3 = root.rotation.eulerAngles;
            root.rotation = Quaternion.Euler(new Vector3(Mathf.LerpAngle(vector3.x, eulerAngles.x, num2), Mathf.LerpAngle(vector3.y, eulerAngles.y, num2), 0f));
            base.ScheduleEvent<TransformTimeSmoothingEvent>(battleCamera);
        }

        [OnEventFire]
        public void InitTimeSmoothing(NodeAddedEvent evt, BattleCameraNode battleCamera)
        {
            CameraComponent camera = battleCamera.camera;
            TransformTimeSmoothingComponent component = new TransformTimeSmoothingComponent {
                Transform = battleCamera.cameraRootTransform.Root,
                UseCorrectionByFrameLeader = true
            };
            battleCamera.Entity.AddComponent(component);
        }

        [OnEventFire]
        public void ResetTimeSmoothing(NodeRemoveEvent evt, BattleCameraNode battleCamera)
        {
            battleCamera.Entity.RemoveComponent<TransformTimeSmoothingComponent>();
        }

        public class BattleCameraNode : NotDeletedEntityNode
        {
            public CameraComponent camera;
            public ApplyCameraTransformComponent applyCameraTransform;
            public CameraTransformDataComponent cameraTransformData;
            public BattleCameraComponent battleCamera;
            public CameraRootTransformComponent cameraRootTransform;
        }
    }
}

