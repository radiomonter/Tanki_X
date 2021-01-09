namespace Tanks.Battle.ClientGraphics.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class TransitionCameraSystem : ECSSystem
    {
        private const float EPS_DIST = 0.01f;
        private static readonly float SPECTATOR_TRANSITION_CAMERA_POSITION_SMOOTHING_RATIO = 10f;
        private static readonly float SPECTATOR_TRANSITION_CAMERA_ROTATION_SMOOTHING_RATIO = 12f;

        [OnEventFire]
        public void InitCamera(NodeAddedEvent evt, SpectatorTransitionCameraNode transitionCameraNode)
        {
            TransitionCameraConfigUnityComponent transitionCameraConfigUnity = transitionCameraNode.transitionCameraConfigUnity;
            if (transitionCameraNode.transitionCamera.CameraSaveData.Type == CameraType.Free)
            {
                TransformData transformData = transitionCameraNode.transitionCamera.CameraSaveData.TransformData;
                Vector3 position = transformData.Position;
                InitCamera(transitionCameraNode, position, transformData.Rotation.eulerAngles, transitionCameraConfigUnity.FlyTimeSec, transitionCameraConfigUnity.FlyHeight);
            }
        }

        [OnEventFire]
        public void InitCamera(NodeAddedEvent evt, SpectatorTransitionCameraNode transitionCameraNode, TankNode tank, [Context, JoinByTank] WeaponNode weapon, [JoinByUser] FollowedUserNode followedUser)
        {
            float flyTimeSec = 0.55f;
            float flyHeight = 1f;
            if (transitionCameraNode.transitionCamera.Spawn)
            {
                SpawnCameraConfigUnityComponent spawnCameraConfigUnity = transitionCameraNode.spawnCameraConfigUnity;
                flyTimeSec = spawnCameraConfigUnity.FlyTimeSec;
                flyHeight = spawnCameraConfigUnity.FlyHeight;
            }
            else
            {
                TransitionCameraConfigUnityComponent transitionCameraConfigUnity = transitionCameraNode.transitionCameraConfigUnity;
                flyTimeSec = transitionCameraConfigUnity.FlyTimeSec;
                flyHeight = transitionCameraConfigUnity.FlyHeight;
            }
            CameraType type = transitionCameraNode.transitionCamera.CameraSaveData.Type;
            if (type == CameraType.Follow)
            {
                this.InitFollowCamera(transitionCameraNode, weapon, tank, flyTimeSec, flyHeight);
            }
            else if (type == CameraType.MouseOrbit)
            {
                TransformData data = MouseOrbitCameraUtils.GetTargetMouseOrbitCameraTransformData(weapon.cameraTarget.TargetObject.transform, transitionCameraNode.transitionCamera.CameraSaveData.MouseOrbitDistance, transitionCameraNode.transitionCamera.CameraSaveData.MouseOrbitTargetRotation);
                InitCamera(transitionCameraNode, data.Position, data.Rotation.eulerAngles, flyTimeSec, flyHeight);
            }
        }

        [OnEventFire]
        public void InitCamera(NodeAddedEvent evt, TransitionCameraNode transitionCameraNode, TankNode tank, [JoinByTank, Context] SpawnTankNode spawnTank, [Context, JoinByTank] WeaponNode weapon)
        {
            SpawnCameraConfigUnityComponent spawnCameraConfigUnity = transitionCameraNode.spawnCameraConfigUnity;
            this.InitFollowCamera(transitionCameraNode, weapon, tank, spawnCameraConfigUnity.FlyTimeSec, spawnCameraConfigUnity.FlyHeight);
        }

        private static void InitCamera(TransitionCameraNode transitionCameraNode, Vector3 position, Vector3 angles, float flyTimeSec, float flyHeight)
        {
            TransitionCameraComponent transitionCamera = transitionCameraNode.transitionCamera;
            CameraTransformDataComponent cameraTransformData = transitionCameraNode.cameraTransformData;
            transitionCamera.P1 = cameraTransformData.Data.Position;
            transitionCamera.P2 = transitionCamera.P1;
            transitionCamera.P4 = position;
            transitionCamera.P3 = transitionCamera.P4;
            float y = ((transitionCamera.P1.y <= transitionCamera.P4.y) ? transitionCamera.P4.y : transitionCamera.P1.y) + flyHeight;
            transitionCamera.P2 = new Vector3(transitionCamera.P2.x, y, transitionCamera.P2.z);
            transitionCamera.P3 = new Vector3(transitionCamera.P3.x, y, transitionCamera.P3.z);
            float accelerationCoeff = 4f / (flyTimeSec * flyTimeSec);
            Vector3 eulerAngles = cameraTransformData.Data.Rotation.eulerAngles;
            transitionCamera.AngleValuesX = new AngleValues(eulerAngles.x, angles.x, accelerationCoeff);
            transitionCamera.AngleValuesY = new AngleValues(eulerAngles.y, angles.y, accelerationCoeff);
            Vector3 vector10 = transitionCamera.P4 - transitionCamera.P1;
            transitionCamera.TotalDistance = vector10.magnitude;
            transitionCamera.Acceleration = transitionCamera.TotalDistance * accelerationCoeff;
        }

        private void InitFollowCamera(TransitionCameraNode cameraNode, WeaponNode weapon, TankNode tank, float flyTimeSec, float flyHeight)
        {
            TransitionCameraComponent transitionCamera = cameraNode.transitionCamera;
            Transform target = weapon.cameraTarget.TargetObject.transform;
            BezierPosition bezierPosition = cameraNode.bezierPosition.BezierPosition;
            CameraOffsetConfigComponent cameraOffsetConfig = cameraNode.cameraOffsetConfig;
            Vector3 cameraOffset = new Vector3(cameraOffsetConfig.XOffset, cameraOffsetConfig.YOffset, cameraOffsetConfig.ZOffset);
            CameraData cameraData = new CameraData();
            Vector3 position = CameraPositionCalculator.CalculateCameraPosition(target, tank.baseRenderer, tank.tankColliders.BoundsCollider.bounds.center, bezierPosition, cameraData, cameraOffset, 0f);
            Vector3 zero = Vector3.zero;
            zero.x = -CameraPositionCalculator.GetPitchAngle(cameraData, bezierPosition) * 57.29578f;
            zero.y = target.transform.rotation.eulerAngles.y;
            InitCamera(cameraNode, position, zero, flyTimeSec, flyHeight);
        }

        private static void UpdateCamera(float dt, TransitionCameraNode cameraNode)
        {
            TransitionCameraComponent transitionCamera = cameraNode.transitionCamera;
            CameraTransformDataComponent cameraTransformData = cameraNode.cameraTransformData;
            if (transitionCamera.TotalDistance == 0f)
            {
                transitionCamera.TransitionComplete = true;
            }
            else if (transitionCamera.Speed < 0f)
            {
                transitionCamera.TransitionComplete = true;
            }
            else
            {
                if ((transitionCamera.Distance > (0.5f * transitionCamera.TotalDistance)) && (transitionCamera.Acceleration > 0f))
                {
                    transitionCamera.Acceleration = -transitionCamera.Acceleration;
                    transitionCamera.AngleValuesX.ReverseAcceleration();
                    transitionCamera.AngleValuesY.ReverseAcceleration();
                }
                float num = transitionCamera.Acceleration * dt;
                transitionCamera.Distance += (transitionCamera.Speed + (0.5f * num)) * dt;
                transitionCamera.Speed += num;
                if (transitionCamera.Distance > transitionCamera.TotalDistance)
                {
                    transitionCamera.Distance = transitionCamera.TotalDistance;
                }
                TransformData data2 = new TransformData {
                    Position = Bezier.PointOnCurve(transitionCamera.Distance / transitionCamera.TotalDistance, transitionCamera.P1, transitionCamera.P2, transitionCamera.P3, transitionCamera.P4),
                    Rotation = Quaternion.Euler(cameraTransformData.Data.Rotation.eulerAngles + new Vector3(transitionCamera.AngleValuesX.Update(dt), transitionCamera.AngleValuesY.Update(dt), 0f))
                };
                cameraTransformData.Data = data2;
                if (Mathf.Abs((float) (transitionCamera.Distance - transitionCamera.TotalDistance)) < 0.01f)
                {
                    transitionCamera.TransitionComplete = true;
                }
            }
        }

        [OnEventFire]
        public void UpdateCamera(TimeUpdateEvent e, SpectatorTransitionCameraNode transitionCameraNode, [JoinAll] Optional<SingleNode<CameraTargetComponent>> target)
        {
            CameraSaveData cameraSaveData = transitionCameraNode.transitionCamera.CameraSaveData;
            if ((cameraSaveData.Type == CameraType.Free) || target.IsPresent())
            {
                UpdateCamera(e.DeltaTime, transitionCameraNode);
                ApplyCameraTransformEvent eventInstance = ApplyCameraTransformEvent.ResetApplyCameraTransformEvent();
                eventInstance.PositionSmoothingRatio = SPECTATOR_TRANSITION_CAMERA_POSITION_SMOOTHING_RATIO;
                eventInstance.RotationSmoothingRatio = SPECTATOR_TRANSITION_CAMERA_ROTATION_SMOOTHING_RATIO;
                eventInstance.DeltaTime = e.DeltaTime;
                base.ScheduleEvent(eventInstance, transitionCameraNode);
                if (transitionCameraNode.transitionCamera.TransitionComplete)
                {
                    CameraType type = cameraSaveData.Type;
                    if (type == CameraType.Follow)
                    {
                        transitionCameraNode.bezierPosition.BezierPosition.SetBaseRatio(cameraSaveData.FollowCameraBezierPositionRatio);
                        transitionCameraNode.bezierPosition.BezierPosition.SetRatioOffset(cameraSaveData.FollowCameraBezierPositionRatioOffset);
                        transitionCameraNode.cameraESM.Esm.ChangeState<CameraStates.CameraFollowState>();
                    }
                    else if (type == CameraType.MouseOrbit)
                    {
                        MouseOrbitCameraComponent mouseOrbitCamera = transitionCameraNode.cameraESM.Esm.ChangeState<CameraStates.CameraOrbitState>().mouseOrbitCamera;
                        mouseOrbitCamera.distance = cameraSaveData.MouseOrbitDistance;
                        mouseOrbitCamera.targetRotation = cameraSaveData.MouseOrbitTargetRotation;
                    }
                    else if (type == CameraType.Free)
                    {
                        transitionCameraNode.cameraESM.Esm.ChangeState<CameraStates.CameraFreeState>();
                    }
                }
            }
        }

        [OnEventFire]
        public void UpdateCamera(TimeUpdateEvent e, TransitionCameraNode transitionCameraNode, [JoinAll] SingleNode<SelfTankComponent> selfTank)
        {
            UpdateCamera(e.DeltaTime, transitionCameraNode);
            base.ScheduleEvent(ApplyCameraTransformEvent.ResetApplyCameraTransformEvent(), transitionCameraNode);
            if (transitionCameraNode.transitionCamera.TransitionComplete)
            {
                transitionCameraNode.cameraESM.Esm.ChangeState<CameraStates.CameraFollowState>();
            }
        }

        public class FollowedUserNode : Node
        {
            public FollowedBattleUserComponent followedBattleUser;
            public UserGroupComponent userGroup;
        }

        public class SpawnTankNode : Node
        {
            public SelfTankComponent selfTank;
            public TankGroupComponent tankGroup;
            public TankSpawnStateComponent tankSpawnState;
        }

        public class SpectatorTransitionCameraNode : TransitionCameraSystem.TransitionCameraNode
        {
            public SpectatorCameraComponent spectatorCamera;
        }

        public class TankNode : Node
        {
            public BaseRendererComponent baseRenderer;
            public TankComponent tank;
            public TankGroupComponent tankGroup;
            public TankCollidersComponent tankColliders;
        }

        public class TransitionCameraNode : Node
        {
            public TransitionCameraComponent transitionCamera;
            public TransitionCameraStateComponent transitionCameraState;
            public CameraOffsetConfigComponent cameraOffsetConfig;
            public SpawnCameraConfigUnityComponent spawnCameraConfigUnity;
            public TransitionCameraConfigUnityComponent transitionCameraConfigUnity;
            public BattleCameraComponent battleCamera;
            public CameraTransformDataComponent cameraTransformData;
            public BezierPositionComponent bezierPosition;
            public CameraESMComponent cameraESM;
        }

        public class WeaponInstanceNode : Node
        {
            public WeaponInstanceComponent weaponInstance;
            public UserGroupComponent userGroup;
            public TankGroupComponent tankGroup;
        }

        public class WeaponNode : Node
        {
            public CameraTargetComponent cameraTarget;
            public UserGroupComponent userGroup;
            public TankGroupComponent tankGroup;
        }
    }
}

