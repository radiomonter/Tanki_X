namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class FreeCameraSystem : ECSSystem
    {
        private static readonly Vector3[] clipDirections = new Vector3[] { Vector3.right, Vector3.left, Vector3.up, Vector3.down, Vector3.forward, Vector3.back };
        private static readonly float CLIP_DISTANCE = 2f;

        private static Vector3 GetClippedPosition(Vector3 currentPosition, Vector3 targetPosition)
        {
            RaycastHit hit;
            for (int i = 0; i < clipDirections.Length; i++)
            {
                Vector3 lhs = targetPosition - currentPosition;
                if ((Vector3.Dot(lhs, clipDirections[i]) > 0f) && Physics.Linecast(currentPosition - clipDirections[i], currentPosition + (clipDirections[i] * CLIP_DISTANCE), out hit, LayerMasks.STATIC))
                {
                    targetPosition -= hit.normal * Vector3.Dot(hit.normal, lhs);
                }
            }
            if (Physics.Linecast(targetPosition + Vector3.up, targetPosition + (Vector3.down * CLIP_DISTANCE), out hit, LayerMasks.STATIC))
            {
                targetPosition.y = hit.point.y + CLIP_DISTANCE;
            }
            return targetPosition;
        }

        [OnEventFire]
        public void Init(NodeAddedEvent evt, FreeCameraNode cameraNode)
        {
            cameraNode.freeCamera.Data = cameraNode.cameraTransformData.Data;
        }

        private static Vector3 NormalizeEuler(Vector3 euler, float xMinAngle, float xMaxAngle)
        {
            euler.x = Mathf.Clamp(((euler.x + 180f) % 360f) - 180f, xMinAngle, xMaxAngle);
            euler.y = (euler.y + 360f) % 360f;
            euler.z = 0f;
            return euler;
        }

        [OnEventFire]
        public unsafe void RecieveInput(TimeUpdateEvent evt, FreeCameraNode cameraNode, [JoinAll] UserNode user)
        {
            float deltaTime = evt.DeltaTime;
            FreeCameraComponent freeCamera = cameraNode.freeCamera;
            CameraTransformDataComponent cameraTransformData = cameraNode.cameraTransformData;
            Quaternion rotation = freeCamera.Data.Rotation;
            Vector3 position = freeCamera.Data.Position;
            bool flag = freeCamera.Data != cameraTransformData.Data;
            float num2 = 1f;
            float num3 = 1f;
            if (InputManager.CheckAction(SpectatorCameraActions.AccelerateMovement))
            {
                num3 *= freeCamera.speedUp;
            }
            if (InputManager.CheckAction(SpectatorCameraActions.SlowMovement))
            {
                num2 *= freeCamera.slowDown;
                num3 *= freeCamera.slowDown;
            }
            if (InputManager.GetActionKeyDown(FreeCameraActions.FIXED_HEIGHT))
            {
                freeCamera.fixedHeight ^= true;
            }
            float num4 = ((!InputManager.CheckAction(FreeCameraActions.UP_TURN) ? 0 : -1) + (!InputManager.CheckAction(FreeCameraActions.DOWN_TURN) ? 0 : 1)) * 0.5f;
            float num5 = ((!InputManager.CheckAction(FreeCameraActions.RIGHT_TURN) ? 0 : 1) + (!InputManager.CheckAction(FreeCameraActions.LEFT_TURN) ? 0 : -1)) * 0.5f;
            if (InputManager.CheckMouseButtonInAllActiveContexts(FreeCameraActions.MOUSE_BUTTON_DOWN, UnityInputConstants.MOUSE_BUTTON_LEFT))
            {
                num4 += -InputManager.GetUnityAxis(UnityInputConstants.MOUSE_Y);
                num5 += InputManager.GetUnityAxis(UnityInputConstants.MOUSE_X);
            }
            Vector3 eulerAngles = rotation.eulerAngles;
            Vector3* vectorPtr1 = &eulerAngles;
            vectorPtr1->x += ((num4 * freeCamera.xRotationSpeed) * num2) * deltaTime;
            Vector3* vectorPtr2 = &eulerAngles;
            vectorPtr2->y += ((num5 * freeCamera.yRotationSpeed) * num2) * deltaTime;
            rotation = Quaternion.Euler(NormalizeEuler(eulerAngles, freeCamera.xMinAngle, freeCamera.xMaxAngle));
            float num6 = (!InputManager.CheckAction(FreeCameraActions.FORWARD_MOVING) ? 0 : 1) + (!InputManager.CheckAction(FreeCameraActions.BACK_MOVING) ? 0 : -1);
            float num7 = (!InputManager.CheckAction(FreeCameraActions.RIGHT_MOVING) ? 0 : 1) + (!InputManager.CheckAction(FreeCameraActions.LEFT_MOVING) ? 0 : -1);
            float num8 = (InputManager.GetAxis(FreeCameraActions.MOUSE_SCROLL_WHEEL_UP, true) - InputManager.GetAxis(FreeCameraActions.MOUSE_SCROLL_WHEEL_DOWN, true)) + (InputManager.GetAxis(FreeCameraActions.UP_MOVING, false) - InputManager.GetAxis(FreeCameraActions.DOWN_MOVING, false));
            Vector3 forward = Vector3.forward;
            Vector3 right = Vector3.right;
            Vector3 up = Vector3.up;
            if (!freeCamera.fixedHeight)
            {
                forward = (Vector3) (rotation * forward);
                right = (Vector3) (rotation * right);
                up = (Vector3) (rotation * up);
            }
            else
            {
                Vector3 euler = rotation.eulerAngles;
                euler.x = 0f;
                Quaternion quaternion2 = Quaternion.Euler(euler);
                forward = (Vector3) (quaternion2 * forward);
                right = (Vector3) (quaternion2 * right);
            }
            TransformData data3 = new TransformData {
                Position = GetClippedPosition(position, position + (((Vector3.ClampMagnitude(((forward * num6) + (right * num7)) + (up * num8), 1f) * freeCamera.flySpeed) * num3) * deltaTime)),
                Rotation = rotation
            };
            ApplyCameraTransformEvent eventInstance = ApplyCameraTransformEvent.ResetApplyCameraTransformEvent();
            if (data3 != freeCamera.Data)
            {
                cameraNode.cameraTransformData.Data = data3;
                freeCamera.Data = data3;
                eventInstance.PositionSmoothingRatio = freeCamera.positionSmoothingSpeed;
                eventInstance.RotationSmoothingRatio = freeCamera.rotationSmoothingSpeed;
            }
            eventInstance.DeltaTime = deltaTime;
            base.ScheduleEvent(eventInstance, cameraNode);
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class FreeCameraNode : Node
        {
            public FreeCameraComponent freeCamera;
            public CameraTransformDataComponent cameraTransformData;
            public BattleCameraComponent battleCamera;
        }

        public class UserNode : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserReadyToBattleComponent userReadyToBattle;
        }
    }
}

