namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class ShaftAimingCameraSystem : ECSSystem
    {
        [OnEventFire]
        public void ApplyKickbackEffect(UpdateEvent evt, ShaftWeaponKickbackNode shaft, [JoinAll] AimingCameraNode cameraNode)
        {
            CameraComponent camera = cameraNode.camera;
            Transform root = cameraNode.cameraRootTransform.Root;
            Vector3 lastPosition = shaft.shaftAimingCameraKickback.LastPosition;
            Quaternion lastRotation = shaft.shaftAimingCameraKickback.LastRotation;
            Vector3 position = shaft.weaponInstance.WeaponInstance.transform.position;
            Quaternion rotation = shaft.weaponInstance.WeaponInstance.transform.rotation;
            shaft.shaftAimingCameraKickback.LastRotation = rotation;
            shaft.shaftAimingCameraKickback.LastPosition = position;
            root.SetPositionSafe((root.position + position) - lastPosition);
            root.SetRotationSafe(root.rotation * (Quaternion.Inverse(lastRotation) * rotation));
        }

        [OnEventFire]
        public void InitManualTargetingCamera(NodeAddedEvent evt, AimingWorkActivationStateNode weapon, InitialCameraNode cameraNode)
        {
            Entity entity = cameraNode.Entity;
            Transform root = cameraNode.cameraRootTransform.Root;
            ShaftAimingCameraComponent component = new ShaftAimingCameraComponent {
                WorldInitialCameraPosition = root.position,
                WorldInitialCameraRotation = root.rotation,
                InitialFOV = cameraNode.camera.FOV
            };
            entity.AddComponent(component);
            if (entity.HasComponent<ApplyCameraTransformComponent>())
            {
                entity.RemoveComponent<ApplyCameraTransformComponent>();
            }
            if (entity.HasComponent<CameraFOVUpdateComponent>())
            {
                entity.RemoveComponent<CameraFOVUpdateComponent>();
            }
            if (entity.HasComponent<ShaftAimingCameraFOVRecoveringComponent>())
            {
                cameraNode.Entity.RemoveComponent<ShaftAimingCameraFOVRecoveringComponent>();
            }
        }

        [OnEventFire]
        public void InterpolateManualTargetingCamera(UpdateEvent evt, AimingWorkActivationStateNode weapon, [JoinAll] AimingCameraNode cameraNode)
        {
            MuzzleVisualAccessor accessor = new MuzzleVisualAccessor(weapon.muzzlePoint);
            CameraComponent camera = cameraNode.camera;
            Transform root = cameraNode.cameraRootTransform.Root;
            ShaftAimingCameraComponent shaftAimingCamera = cameraNode.shaftAimingCamera;
            float t = Mathf.Clamp01(weapon.shaftAimingWorkActivationState.ActivationTimer / weapon.shaftStateConfig.ActivationToWorkingTransitionTimeSec);
            MuzzleLogicAccessor accessor2 = new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance);
            root.SetPositionSafe(Vector3.Lerp(shaftAimingCamera.WorldInitialCameraPosition, accessor2.GetBarrelOriginWorld(), t));
            Quaternion quaternion2 = Quaternion.LookRotation(accessor.GetFireDirectionWorld(), accessor.GetUpDirectionWorld());
            weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle = Mathf.Clamp(weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle, -weapon.shaftAimingRotationConfig.AimingOffsetClipping, weapon.shaftAimingRotationConfig.AimingOffsetClipping);
            Vector3 eulerAngles = quaternion2.eulerAngles;
            root.SetRotationSafe(Quaternion.Slerp(shaftAimingCamera.WorldInitialCameraRotation, Quaternion.Euler(eulerAngles.x, eulerAngles.y + weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle, eulerAngles.z), t));
            camera.FOV = Mathf.Lerp(shaftAimingCamera.InitialFOV, weapon.shaftAimingCameraConfigEffect.ActivationStateTargetFov, t);
        }

        [OnEventFire]
        public void RecoverFOV(UpdateEvent evt, ShaftAimingCameraEffectConfigNode weapon, [JoinAll] CameraRecoveringNode cameraNode)
        {
            CameraComponent camera = cameraNode.camera;
            float recoveringFovSpeed = weapon.shaftAimingCameraConfigEffect.RecoveringFovSpeed;
            float optimalFOV = cameraNode.battleCamera.OptimalFOV;
            camera.FOV += recoveringFovSpeed * evt.DeltaTime;
            if (camera.FOV >= optimalFOV)
            {
                camera.FOV = optimalFOV;
                Entity entity = cameraNode.Entity;
                entity.RemoveComponent<ShaftAimingCameraFOVRecoveringComponent>();
                entity.AddComponent<CameraFOVUpdateComponent>();
            }
        }

        [OnEventFire]
        public void ResetCamera(NodeAddedEvent evt, AimingIdleStateNode weapon, AimingCameraNode cameraNode)
        {
            ShaftAimingCameraComponent shaftAimingCamera = cameraNode.shaftAimingCamera;
            CameraTransformDataComponent cameraTransformData = cameraNode.cameraTransformData;
            CameraComponent camera = cameraNode.camera;
            Transform root = cameraNode.cameraRootTransform.Root;
            root.SetPositionSafe(cameraTransformData.Data.Position);
            root.SetRotationSafe(cameraTransformData.Data.Rotation);
            Entity entity = cameraNode.Entity;
            entity.RemoveComponent<ShaftAimingCameraComponent>();
            entity.AddComponent<ShaftAimingCameraFOVRecoveringComponent>();
            entity.AddComponent<ApplyCameraTransformComponent>();
        }

        [OnEventFire]
        public void ResetKickBackEffect(NodeAddedEvent evt, AimingIdleKickbackStateNode shaft)
        {
            shaft.Entity.RemoveComponent<ShaftAimingCameraKickbackComponent>();
        }

        [OnEventFire]
        public void SetKickBackEffectForCameraOnAimingShot(ShaftAimingShotPrepareEvent evt, ShaftWeaponNode shaft)
        {
            shaft.Entity.AddComponent(new ShaftAimingCameraKickbackComponent(shaft.weaponInstance.WeaponInstance.transform.position, shaft.weaponInstance.WeaponInstance.transform.rotation));
        }

        [OnEventFire]
        public unsafe void UpdateCameraAtWorkingState(WeaponRotationUpdateShaftAimingCameraEvent e, AimingWorkingStateNode weapon, [JoinAll] AimingCameraNode cameraNode)
        {
            Transform root = cameraNode.cameraRootTransform.Root;
            MuzzleLogicAccessor accessor = new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance);
            ShaftAimingCameraConfigEffectComponent shaftAimingCameraConfigEffect = weapon.shaftAimingCameraConfigEffect;
            ShaftAimingWorkingStateComponent shaftAimingWorkingState = weapon.shaftAimingWorkingState;
            root.SetPositionSafe(accessor.GetBarrelOriginWorld());
            root.SetRotationSafe(Quaternion.LookRotation(shaftAimingWorkingState.WorkingDirection, accessor.GetUpDirectionWorld()));
            Vector3 eulerAngles = root.eulerAngles;
            weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle = Mathf.Clamp(weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle, -weapon.shaftAimingRotationConfig.AimingOffsetClipping, weapon.shaftAimingRotationConfig.AimingOffsetClipping);
            float num = Mathf.Clamp(weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle, -weapon.shaftAimingRotationConfig.MaxAimingCameraOffset, weapon.shaftAimingRotationConfig.MaxAimingCameraOffset);
            Vector3* vectorPtr1 = &eulerAngles;
            vectorPtr1->y += num;
            Vector3* vectorPtr2 = &eulerAngles;
            vectorPtr2->x -= weapon.weaponRotationControl.MouseShaftAimCumulativeVerticalAngle;
            root.eulerAngles = eulerAngles;
            cameraNode.camera.FOV = Mathf.Lerp(shaftAimingCameraConfigEffect.ActivationStateTargetFov, shaftAimingCameraConfigEffect.WorkingStateMinFov, shaftAimingWorkingState.InitialEnergy - weapon.weaponEnergy.Energy);
        }

        [Not(typeof(ApplyCameraTransformComponent))]
        public class AimingCameraNode : ShaftAimingCameraSystem.InitialCameraNode
        {
            public CameraTransformDataComponent cameraTransformData;
            public ShaftAimingCameraComponent shaftAimingCamera;
        }

        public class AimingIdleKickbackStateNode : ShaftAimingCameraSystem.AimingIdleStateNode
        {
            public ShaftAimingCameraKickbackComponent shaftAimingCameraKickback;
        }

        public class AimingIdleStateNode : ShaftAimingCameraSystem.ShaftWeaponNode
        {
            public ShaftIdleStateComponent shaftIdleState;
            public ShaftAimingCameraConfigEffectComponent shaftAimingCameraConfigEffect;
        }

        public class AimingWorkActivationStateNode : ShaftAimingCameraSystem.ShaftWeaponNode
        {
            public ShaftAimingWorkActivationStateComponent shaftAimingWorkActivationState;
            public ShaftAimingCameraConfigEffectComponent shaftAimingCameraConfigEffect;
            public ShaftAimingRotationConfigComponent shaftAimingRotationConfig;
            public ShaftStateConfigComponent shaftStateConfig;
            public WeaponRotationControlComponent weaponRotationControl;
        }

        public class AimingWorkingStateNode : ShaftAimingCameraSystem.ShaftWeaponNode
        {
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
            public ShaftAimingCameraConfigEffectComponent shaftAimingCameraConfigEffect;
            public ShaftAimingRotationConfigComponent shaftAimingRotationConfig;
            public WeaponEnergyComponent weaponEnergy;
            public WeaponRotationControlComponent weaponRotationControl;
        }

        [Not(typeof(CameraFOVUpdateComponent))]
        public class CameraRecoveringNode : ShaftAimingCameraSystem.InitialCameraNode
        {
            public ShaftAimingCameraFOVRecoveringComponent shaftAimingCameraFOVRecovering;
        }

        public class InitialCameraNode : Node
        {
            public BattleCameraComponent battleCamera;
            public CameraRootTransformComponent cameraRootTransform;
            public CameraComponent camera;
        }

        public class ShaftAimingCameraEffectConfigNode : Node
        {
            public ShaftAimingCameraConfigEffectComponent shaftAimingCameraConfigEffect;
            public ShaftStateControllerComponent shaftStateController;
        }

        public class ShaftWeaponKickbackNode : ShaftAimingCameraSystem.ShaftWeaponNode
        {
            public ShaftAimingCameraKickbackComponent shaftAimingCameraKickback;
        }

        public class ShaftWeaponNode : Node
        {
            public ShaftStateControllerComponent shaftStateController;
            public WeaponInstanceComponent weaponInstance;
            public MuzzlePointComponent muzzlePoint;
        }
    }
}

