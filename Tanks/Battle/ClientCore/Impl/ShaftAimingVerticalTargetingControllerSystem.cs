namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class ShaftAimingVerticalTargetingControllerSystem : ECSSystem
    {
        private bool CheckManualTargetingActivity(WeaponRotationControlComponent rotation, VerticalSectorsTargetingComponent verticalSectors, ShaftAimingWorkingStateComponent working, bool isInputActive)
        {
            if (rotation.IsRotating())
            {
                return true;
            }
            if (!isInputActive)
            {
                return false;
            }
            float verticalAngle = working.VerticalAngle;
            return ((verticalAngle != verticalSectors.VAngleUp) ? (verticalAngle != -verticalSectors.VAngleDown) : false);
        }

        [OnEventFire]
        public void UpdateVerticalTargetingAngle(WeaponRotationUpdateVerticalEvent e, AimingVerticalTargetingControllerNode weapon, [JoinByUser] SingleNode<MouseControlStateHolderComponent> mouseControlStateHolder)
        {
            float min = -weapon.verticalSectorsTargeting.VAngleDown;
            float vAngleUp = weapon.verticalSectorsTargeting.VAngleUp;
            float maxVerticalSpeed = weapon.shaftAimingSpeed.MaxVerticalSpeed;
            float verticalAcceleration = weapon.shaftAimingSpeed.VerticalAcceleration;
            int shaftElevationDirectionByKeyboard = weapon.weaponRotationControl.ShaftElevationDirectionByKeyboard;
            bool mouseControlEnable = mouseControlStateHolder.component.MouseControlEnable;
            if (!mouseControlEnable)
            {
                weapon.weaponRotationControl.MouseShaftAimCumulativeVerticalAngle = 0f;
            }
            else
            {
                shaftElevationDirectionByKeyboard = (int) Mathf.Sign(weapon.weaponRotationControl.MouseShaftAimCumulativeVerticalAngle);
                weapon.weaponRotationControl.MouseShaftAimCumulativeVerticalAngle = Mathf.Clamp(weapon.weaponRotationControl.MouseShaftAimCumulativeVerticalAngle, min - weapon.shaftAimingWorkingState.VerticalAngle, vAngleUp - weapon.shaftAimingWorkingState.VerticalAngle);
            }
            bool isInputActive = false;
            if (weapon.shaftAimingWorkingState.VerticalElevationDir != shaftElevationDirectionByKeyboard)
            {
                weapon.shaftAimingWorkingState.VerticalSpeed = 0f;
                weapon.shaftAimingWorkingState.VerticalElevationDir = shaftElevationDirectionByKeyboard;
                isInputActive = shaftElevationDirectionByKeyboard != 0;
            }
            else
            {
                weapon.shaftAimingWorkingState.VerticalElevationDir = shaftElevationDirectionByKeyboard;
                weapon.shaftAimingWorkingState.VerticalSpeed += verticalAcceleration * e.DeltaTime;
                weapon.shaftAimingWorkingState.VerticalSpeed = Mathf.Clamp(weapon.shaftAimingWorkingState.VerticalSpeed, 0f, maxVerticalSpeed);
                float num6 = (shaftElevationDirectionByKeyboard * weapon.shaftAimingWorkingState.VerticalSpeed) * e.DeltaTime;
                if (!mouseControlEnable)
                {
                    isInputActive = num6 != 0f;
                    weapon.shaftAimingWorkingState.VerticalAngle += num6;
                    weapon.shaftAimingWorkingState.VerticalAngle = Mathf.Clamp(weapon.shaftAimingWorkingState.VerticalAngle, min, vAngleUp);
                }
                else if (!weapon.weaponRotationControl.BlockRotate)
                {
                    num6 = (shaftElevationDirectionByKeyboard <= 0) ? Mathf.Clamp(num6, weapon.weaponRotationControl.MouseShaftAimCumulativeVerticalAngle, 0f) : Mathf.Clamp(num6, 0f, weapon.weaponRotationControl.MouseShaftAimCumulativeVerticalAngle);
                    isInputActive = num6 != 0f;
                    weapon.weaponRotationControl.MouseShaftAimCumulativeVerticalAngle -= num6;
                    weapon.shaftAimingWorkingState.VerticalAngle += num6;
                    weapon.shaftAimingWorkingState.VerticalAngle = Mathf.Clamp(weapon.shaftAimingWorkingState.VerticalAngle, min, vAngleUp);
                }
            }
            MuzzleLogicAccessor accessor = new MuzzleLogicAccessor(weapon.muzzlePoint, weapon.weaponInstance);
            weapon.shaftAimingWorkingState.WorkingDirection = (Vector3) (Quaternion.AngleAxis(weapon.shaftAimingWorkingState.VerticalAngle, accessor.GetLeftDirectionWorld()) * accessor.GetFireDirectionWorld());
            weapon.shaftAimingWorkingState.IsActive = this.CheckManualTargetingActivity(weapon.weaponRotationControl, weapon.verticalSectorsTargeting, weapon.shaftAimingWorkingState, isInputActive);
        }

        public class AimingVerticalTargetingControllerNode : Node
        {
            public ShaftAimingSpeedComponent shaftAimingSpeed;
            public VerticalSectorsTargetingComponent verticalSectorsTargeting;
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
            public MuzzlePointComponent muzzlePoint;
            public WeaponRotationControlComponent weaponRotationControl;
            public WeaponInstanceComponent weaponInstance;
        }
    }
}

