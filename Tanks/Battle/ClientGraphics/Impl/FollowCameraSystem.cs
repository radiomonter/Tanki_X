namespace Tanks.Battle.ClientGraphics.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class FollowCameraSystem : ECSSystem
    {
        private static readonly float SPECTATOR_FOLLOW_CAMERA_POSITION_SMOOTHING_RATIO = 10f;
        private static readonly float SPECTATOR_FOLLOW_CAMERA_ROTATION_RATIO = 12f;
        public static float BEZIER_SPEED_BY_MOUSE_VERTICAL = 6f;
        public static float MOUSE_WHEEL_RATIO = 0.032f;

        [OnEventComplete]
        public void Follow(CameraFollowEvent e, SingleNode<WeaponInstanceComponent> weapon, [JoinAll] SmoothingCameraNode battleCamera)
        {
            battleCamera.transformTimeSmoothingData.LastPosition = battleCamera.cameraRootTransform.Root.position;
            battleCamera.transformTimeSmoothingData.LastRotation = battleCamera.cameraRootTransform.Root.rotation;
        }

        [OnEventFire]
        public void Follow(CameraFollowEvent e, CameraTargetNode cameraTargetNode, [JoinByTank] TankNode tank, [JoinAll] CameraNode cameraNode, [JoinAll] Optional<SingleNode<FollowCameraComponent>> followCameraOptionalNode)
        {
            CameraTransformDataComponent cameraTransformData = cameraNode.cameraTransformData;
            CameraOffsetConfigComponent cameraOffsetConfig = cameraNode.cameraOffsetConfig;
            Vector3 cameraOffset = new Vector3(cameraOffsetConfig.XOffset, cameraOffsetConfig.YOffset, cameraOffsetConfig.ZOffset);
            BezierPosition bezierPosition = cameraNode.bezierPosition.BezierPosition;
            cameraTransformData.Data = CameraPositionCalculator.GetTargetFollowCameraTransformData(cameraTargetNode.cameraTarget.TargetObject.transform, tank.baseRenderer, tank.tankColliders.BoundsCollider.bounds.center, bezierPosition, cameraOffset);
            cameraNode.cameraRootTransform.Root.SetPositionSafe(cameraTransformData.Data.Position);
            cameraNode.cameraRootTransform.Root.SetRotationSafe(cameraTransformData.Data.Rotation);
        }

        private float GetHeightChangeDir(bool mouseInvert)
        {
            float axis = InputManager.GetAxis(CameraRotationActions.ROTATE_CAMERA_UP_LEFT, false);
            float num2 = InputManager.GetAxis(CameraRotationActions.ROTATE_CAMERA_DOWN_RIGHT, false);
            float num3 = InputManager.GetAxis(CameraRotationActions.MOUSEWHEEL_MOVE, false) * MOUSE_WHEEL_RATIO;
            if (mouseInvert)
            {
                num3 *= -1f;
            }
            return ((axis - num2) + num3);
        }

        [OnEventFire]
        public void InitFollowCameraData(NodeAddedEvent evt, FollowCameraNode cameraNode)
        {
            cameraNode.followCamera.cameraData = new CameraData();
        }

        private void UpdateBezierPosition(BezierPosition bezierPosition, float verticalCameraSpeed, float dt, float vertical, bool mouseInvert)
        {
            float axisOrKey = InputManager.GetAxisOrKey(CameraRotationActions.ROTATE_CAMERA_UP_LEFT);
            float num2 = InputManager.GetAxisOrKey(CameraRotationActions.ROTATE_CAMERA_DOWN_RIGHT);
            float num3 = InputManager.GetAxis(CameraRotationActions.MOUSEWHEEL_MOVE, false) * MOUSE_WHEEL_RATIO;
            if (mouseInvert)
            {
                num3 *= -1f;
            }
            float num5 = (((axisOrKey - num2) * verticalCameraSpeed) * dt) + num3;
            bezierPosition.SetBaseRatio(bezierPosition.GetBaseRatio() + num5);
            float num6 = BEZIER_SPEED_BY_MOUSE_VERTICAL / 180f;
            bezierPosition.SetRatioOffset(bezierPosition.GetRatioOffset() + (vertical * num6));
        }

        private void UpdateFollowCameraData(float deltaTime, FollowCameraNode cameraNode, CameraTargetNode targetNode, TankNode tank, MouseControlStateHolderComponent mouseControlStateHolder)
        {
            GameObject targetObject = targetNode.cameraTarget.TargetObject;
            if (targetObject != null)
            {
                CameraTransformDataComponent cameraTransformData = cameraNode.cameraTransformData;
                Transform target = targetObject.transform;
                FollowCameraComponent followCamera = cameraNode.followCamera;
                BezierPosition bezierPosition = cameraNode.bezierPosition.BezierPosition;
                CameraData cameraData = followCamera.cameraData;
                this.UpdateBezierPosition(bezierPosition, followCamera.verticalCameraSpeed, deltaTime, targetNode.weaponRotationControl.MouseRotationCumulativeVerticalAngle, mouseControlStateHolder.MouseVerticalInverted);
                targetNode.weaponRotationControl.MouseRotationCumulativeVerticalAngle = 0f;
                float mouseRotationCumulativeHorizontalAngle = targetNode.weaponRotationControl.MouseRotationCumulativeHorizontalAngle;
                if (Mathf.Abs(MathUtil.ClampAngle180(Vector3.Angle(target.up, Vector3.up))) > 90f)
                {
                    mouseRotationCumulativeHorizontalAngle *= -1f;
                }
                bool mouse = targetNode.weaponRotationControl.MouseRotationCumulativeHorizontalAngle != 0f;
                CameraOffsetConfigComponent cameraOffsetConfig = cameraNode.cameraOffsetConfig;
                Vector3 cameraOffset = new Vector3(cameraOffsetConfig.XOffset, cameraOffsetConfig.YOffset, cameraOffsetConfig.ZOffset);
                Vector3 cameraCalculated = CameraPositionCalculator.CalculateCameraPosition(target, tank.baseRenderer, tank.tankColliders.BoundsCollider.bounds.center, bezierPosition, cameraData, cameraOffset, mouseRotationCumulativeHorizontalAngle);
                Vector3 rotation = cameraTransformData.Data.Rotation.eulerAngles * 0.01745329f;
                CameraPositionCalculator.CalculatePitchMovement(ref rotation, bezierPosition, deltaTime, cameraData, mouse);
                CameraPositionCalculator.CalculateYawMovement((Vector3) (Quaternion.Euler(new Vector3(0f, mouseRotationCumulativeHorizontalAngle, 0f)) * target.forward), ref rotation, deltaTime, cameraData, mouse);
                CameraPositionCalculator.SmoothReturnRoll(ref rotation, followCamera.rollReturnSpeedDegPerSec, deltaTime);
                TransformData data4 = new TransformData {
                    Position = CameraPositionCalculator.CalculateLinearMovement(deltaTime, cameraCalculated, cameraTransformData.Data.Position, cameraData, target, mouse),
                    Rotation = Quaternion.Euler(rotation * 57.29578f)
                };
                cameraTransformData.Data = data4;
            }
        }

        [OnEventComplete]
        public void UpdateFollowCameraData(UpdateEvent e, FollowCameraNode cameraNode, [JoinAll] CameraTargetNode targetNode, [JoinByTank] TankNode tank, [JoinAll] SingleNode<SelfTankComponent> selfTank, [JoinByUser] SingleNode<MouseControlStateHolderComponent> mouseControlStateHolderNode)
        {
            this.UpdateFollowCameraData(e.DeltaTime, cameraNode, targetNode, tank, mouseControlStateHolderNode.component);
            base.ScheduleEvent(ApplyCameraTransformEvent.ResetApplyCameraTransformEvent(), cameraNode);
        }

        [OnEventFire]
        public void UpdateFollowCameraData(UpdateEvent e, FollowSpectratorCameraNode cameraNode, [JoinAll] CameraTargetNode targetNode, [JoinByTank] TankNode tank, [JoinAll] SingleNode<SelfUserComponent> selfUser, [JoinByUser] SingleNode<MouseControlStateHolderComponent> mouseControlStateHolderNode)
        {
            this.UpdateFollowCameraData(e.DeltaTime, cameraNode, targetNode, tank, mouseControlStateHolderNode.component);
            ApplyCameraTransformEvent eventInstance = ApplyCameraTransformEvent.ResetApplyCameraTransformEvent();
            eventInstance.PositionSmoothingRatio = SPECTATOR_FOLLOW_CAMERA_POSITION_SMOOTHING_RATIO;
            eventInstance.RotationSmoothingRatio = SPECTATOR_FOLLOW_CAMERA_ROTATION_RATIO;
            eventInstance.DeltaTime = e.DeltaTime;
            base.ScheduleEvent(eventInstance, cameraNode);
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class BattleCameraNode : Node
        {
            public CameraComponent camera;
            public CameraRootTransformComponent cameraRootTransform;
            public ApplyCameraTransformComponent applyCameraTransform;
            public CameraTransformDataComponent cameraTransformData;
            public BattleCameraComponent battleCamera;
        }

        public class CameraNode : Node
        {
            public BattleCameraComponent battleCamera;
            public CameraRootTransformComponent cameraRootTransform;
            public CameraComponent camera;
            public CameraTransformDataComponent cameraTransformData;
            public CameraOffsetConfigComponent cameraOffsetConfig;
            public BezierPositionComponent bezierPosition;
            public CameraESMComponent cameraEsm;
        }

        public class CameraTargetNode : Node
        {
            public CameraTargetComponent cameraTarget;
            public WeaponRotationControlComponent weaponRotationControl;
            public TankGroupComponent tankGroup;
        }

        public class FollowCameraNode : FollowCameraSystem.CameraNode
        {
            public FollowCameraComponent followCamera;
        }

        public class FollowSpectratorCameraNode : FollowCameraSystem.FollowCameraNode
        {
            public SpectatorCameraComponent spectatorCamera;
        }

        public class FreeCameraNode : FollowCameraSystem.CameraNode
        {
            public FreeCameraComponent freeCamera;
        }

        public class MouseOrbitCameraNode : FollowCameraSystem.CameraNode
        {
            public MouseOrbitCameraComponent mouseOrbitCamera;
        }

        public class SmoothingCameraNode : FollowCameraSystem.BattleCameraNode
        {
            public TransformTimeSmoothingDataComponent transformTimeSmoothingData;
        }

        public class TankNode : Node
        {
            public BaseRendererComponent baseRenderer;
            public TankComponent tank;
            public TankGroupComponent tankGroup;
            public TankCollidersComponent tankColliders;
        }

        public class UserWeaponNode : Node
        {
            public UserGroupComponent userGroup;
            public WeaponInstanceComponent weaponInstance;
        }
    }
}

