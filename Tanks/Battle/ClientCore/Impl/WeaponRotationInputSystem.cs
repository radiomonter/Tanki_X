namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class WeaponRotationInputSystem : ECSSystem
    {
        [OnEventFire]
        public void Deactivate(NodeRemoveEvent e, SelfTankNode tank, [JoinByTank] WeaponNode weapon)
        {
            weapon.weaponRotationControl.Control = 0f;
        }

        private float GetMouseXDelta(GameObject weaponInstance, float mouseSpeed, float deltaTime)
        {
            float num = (InputManager.GetAxis(CameraRotationActions.MOUSEX_ROTATE, false) * mouseSpeed) / deltaTime;
            if (Mathf.Abs(MathUtil.ClampAngle180(Vector3.Angle(weaponInstance.transform.up, Vector3.up))) > 90f)
            {
                num *= -1f;
            }
            return num;
        }

        [OnEventFire]
        public void OnWeaponUpdateEvent(UpdateEvent e, WeaponNode weapon, [JoinByTank] SelfTankNode tank, [JoinByUser] SingleNode<MouseControlStateHolderComponent> mouseControlStateHolder, [JoinAll] Optional<SingleNode<BattleShaftAimingStateComponent>> shaftAimingState)
        {
            WeaponRotationControlComponent weaponRotationControl = weapon.weaponRotationControl;
            float mouseSpeed = ((Camera.main.fieldOfView * mouseControlStateHolder.component.MouseSensivity) * 1f) / ((float) Screen.height);
            if (shaftAimingState.IsPresent())
            {
                if (!mouseControlStateHolder.component.MouseControlEnable)
                {
                    weaponRotationControl.MouseRotationCumulativeHorizontalAngle = 0f;
                    int num4 = !InputManager.CheckAction(ShaftAimingActions.AIMING_UP) ? 0 : 1;
                    weaponRotationControl.ShaftElevationDirectionByKeyboard = num4 - (!InputManager.CheckAction(ShaftAimingActions.AIMING_DOWN) ? 0 : 1);
                    weaponRotationControl.Control = Mathf.Clamp((float) (InputManager.GetAxis(WeaponActions.WEAPON_RIGHT, false) - InputManager.GetAxis(WeaponActions.WEAPON_LEFT, false)), (float) -1f, (float) 1f);
                    weaponRotationControl.CenteringControl = InputManager.CheckAction(WeaponActions.WEAPON_CENTER);
                }
                else
                {
                    float axis = InputManager.GetAxis(CameraRotationActions.MOUSEY_MOVE_SHAFT_AIM, false);
                    if (mouseControlStateHolder.component.MouseVerticalInverted)
                    {
                        axis *= -1f;
                    }
                    weaponRotationControl.MouseShaftAimCumulativeVerticalAngle += (axis * mouseSpeed) / e.DeltaTime;
                    float num3 = this.GetMouseXDelta(weapon.weaponInstance.WeaponInstance, mouseSpeed, e.DeltaTime);
                    weaponRotationControl.MouseRotationCumulativeHorizontalAngle = MathUtil.ClampAngle180(weaponRotationControl.MouseRotationCumulativeHorizontalAngle + num3);
                    weaponRotationControl.Centering = false;
                    weaponRotationControl.CenteringControl = false;
                    weaponRotationControl.BlockRotate = InputManager.CheckAction(WeaponActions.WEAPON_LOCK_ROTATION);
                }
            }
            else
            {
                weaponRotationControl.Control = Mathf.Clamp((float) (InputManager.GetAxis(WeaponActions.WEAPON_RIGHT, false) - InputManager.GetAxis(WeaponActions.WEAPON_LEFT, false)), (float) -1f, (float) 1f);
                weaponRotationControl.CenteringControl = InputManager.CheckAction(WeaponActions.WEAPON_CENTER);
                if ((weaponRotationControl.Control != 0f) || weaponRotationControl.CenteringControl)
                {
                    mouseControlStateHolder.component.MouseControlEnable = false;
                    weaponRotationControl.MouseRotationCumulativeHorizontalAngle = 0f;
                }
                else
                {
                    float num10 = (InputManager.GetAxis(CameraRotationActions.MOUSEY_ROTATE, false) * mouseSpeed) / e.DeltaTime;
                    float num11 = this.GetMouseXDelta(weapon.weaponInstance.WeaponInstance, mouseSpeed, e.DeltaTime);
                    if ((num11 != 0f) || (num10 != 0f))
                    {
                        mouseControlStateHolder.component.MouseControlEnable = true;
                    }
                    if (mouseControlStateHolder.component.MouseVerticalInverted)
                    {
                        num10 *= -1f;
                    }
                    weaponRotationControl.MouseRotationCumulativeHorizontalAngle = MathUtil.ClampAngle180(weaponRotationControl.MouseRotationCumulativeHorizontalAngle + num11);
                    weaponRotationControl.MouseRotationCumulativeVerticalAngle += num10;
                    weaponRotationControl.BlockRotate = InputManager.CheckAction(WeaponActions.WEAPON_LOCK_ROTATION);
                    weaponRotationControl.CenteringControl = InputManager.CheckAction(WeaponActions.WEAPON_CENTER_BY_MOUSE);
                }
            }
            weaponRotationControl.Centering |= weaponRotationControl.CenteringControl;
            if (weaponRotationControl.Centering && ((weaponRotationControl.Control != 0f) || (weaponRotationControl.Rotation == 0f)))
            {
                weaponRotationControl.Centering = false;
            }
        }

        [OnEventFire]
        public void SwitchContextToAiming(NodeAddedEvent evt, SingleNode<ShaftAimingWorkActivationStateComponent> shaftNode, [JoinByUser] SingleNode<MouseControlStateHolderComponent> mouseControlStateHolder, [JoinByUser] SingleNode<SelfBattleUserComponent> selfBattleUser)
        {
            mouseControlStateHolder.component.MouseControlEnable = mouseControlStateHolder.component.MouseControlAllowed && InputManager.GetMouseButton(UnityInputConstants.MOUSE_BUTTON_LEFT);
        }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }

        public class SelfTankNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankMovableComponent tankMovable;
            public SelfTankComponent selfTank;
        }

        public class WeaponNode : Node
        {
            public TankGroupComponent tankGroup;
            public WeaponInstanceComponent weaponInstance;
            public WeaponRotationControlComponent weaponRotationControl;
        }
    }
}

