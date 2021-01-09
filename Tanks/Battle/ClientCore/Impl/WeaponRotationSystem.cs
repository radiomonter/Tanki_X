namespace Tanks.Battle.ClientCore.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class WeaponRotationSystem : ECSSystem
    {
        private float ClampAngle(float value)
        {
            while (value < 0f)
            {
                value += 360f;
            }
            while (value >= 360f)
            {
                value -= 360f;
            }
            return value;
        }

        [OnEventFire]
        public void OnTankInactive(NodeRemoveEvent e, SelfTankNode selfTank, [JoinByTank] WeaponNode weapon)
        {
            WeaponRotationControlComponent weaponRotationControl = weapon.weaponRotationControl;
            if (weaponRotationControl.EffectiveControl != 0f)
            {
                weaponRotationControl.PrevEffectiveControl = 0f;
                weaponRotationControl.EffectiveControl = 0f;
                base.ScheduleEvent<WeaponRotationControlChangedEvent>(weapon);
            }
        }

        [OnEventFire]
        public void OnTankSpawn(NodeAddedEvent e, TankIncarnationNode selfTank, [JoinByTank] WeaponNode weapon)
        {
            weapon.weaponRotationControl.MouseRotationCumulativeHorizontalAngle = 0f;
        }

        [OnEventFire]
        public void UpdateRemoteWeapon(TimeUpdateEvent e, MovableRemoteTankNode remoteTank, [JoinByTank] WeaponNode weapon)
        {
            this.UpdateWeaponRotation(weapon, null, e.DeltaTime, true);
        }

        [OnEventFire]
        public void UpdateSelfCommonWeapon(TimeUpdateEvent e, SimpleWeaponNode weapon, [JoinByTank] SelfTankNode tank, [JoinByUser] SingleNode<MouseControlStateHolderComponent> mouseControlStateHolder)
        {
            float deltaTime = e.DeltaTime;
            base.ScheduleEvent(BaseWeaponRotationUpdateDeltaTimeEvent<WeaponRotationUpdateGyroscopeEvent>.GetInstance(deltaTime), weapon);
            this.UpdateWeaponRotation(weapon, mouseControlStateHolder, deltaTime, false);
        }

        [OnEventFire]
        public void UpdateSelfShaftWeapon(TimeUpdateEvent e, ShaftWeaponNode weapon, [JoinByTank] SelfTankNode tank, [JoinByUser] SingleNode<MouseControlStateHolderComponent> mouseControlStateHolder)
        {
            float deltaTime = e.DeltaTime;
            base.ScheduleEvent(BaseWeaponRotationUpdateDeltaTimeEvent<WeaponRotationUpdateGyroscopeEvent>.GetInstance(deltaTime), weapon);
            this.UpdateWeaponRotation(weapon, mouseControlStateHolder, deltaTime, false);
            base.ScheduleEvent(BaseWeaponRotationUpdateDeltaTimeEvent<WeaponRotationUpdateVerticalEvent>.GetInstance(deltaTime), weapon);
            base.ScheduleEvent(BaseWeaponRotationUpdateDeltaTimeEvent<WeaponRotationUpdateShaftAimingCameraEvent>.GetInstance(deltaTime), weapon);
            base.ScheduleEvent<WeaponRotationUpdateAimEvent>(weapon);
        }

        private void UpdateWeaponRotation(WeaponNode weapon, SingleNode<MouseControlStateHolderComponent> mouseControlStateHolder, float dt, bool rem = false)
        {
            WeaponRotationControlComponent weaponRotationControl = weapon.weaponRotationControl;
            weaponRotationControl.EffectiveControl = !weaponRotationControl.Centering ? weaponRotationControl.Control : ((weaponRotationControl.Rotation >= 180f) ? ((float) 1) : ((float) (-1)));
            WeaponGyroscopeRotationComponent weaponGyroscopeRotation = weapon.weaponGyroscopeRotation;
            bool flag = (mouseControlStateHolder != null) && mouseControlStateHolder.component.MouseControlEnable;
            if (flag && (weaponRotationControl.BlockRotate && !weaponRotationControl.ForceGyroscopeEnabled))
            {
                weaponRotationControl.MouseRotationCumulativeHorizontalAngle -= weaponGyroscopeRotation.DeltaAngleOfHullRotation;
                weaponRotationControl.MouseRotationCumulativeHorizontalAngle = MathUtil.ClampAngle(weaponRotationControl.MouseRotationCumulativeHorizontalAngle * 0.01745329f) * 57.29578f;
                weaponGyroscopeRotation.DeltaAngleOfHullRotation = 0f;
            }
            float num2 = (weapon.weaponRotation.Acceleration * weaponGyroscopeRotation.WeaponTurnCoeff) * dt;
            float max = weapon.weaponRotation.Speed * weaponGyroscopeRotation.WeaponTurnCoeff;
            float num4 = 0f;
            float f = 0f;
            bool flag2 = false;
            while (true)
            {
                float mouseRotationCumulativeHorizontalAngle = weaponRotationControl.MouseRotationCumulativeHorizontalAngle;
                float effectiveControl = weaponRotationControl.EffectiveControl;
                weaponRotationControl.Speed += num2;
                weaponRotationControl.Speed = Mathf.Clamp(weaponRotationControl.Speed, 0f, max);
                num4 = weaponRotationControl.Speed * dt;
                if (!flag)
                {
                    f = effectiveControl * num4;
                    if (weaponRotationControl.ForceGyroscopeEnabled)
                    {
                        f -= weaponGyroscopeRotation.DeltaAngleOfHullRotation;
                    }
                }
                else
                {
                    if (weaponRotationControl.ForceGyroscopeEnabled)
                    {
                        if (!weaponRotationControl.BlockRotate)
                        {
                            if (weaponRotationControl.Centering)
                            {
                                f = Mathf.Clamp(-MathUtil.ClampAngle180(weaponRotationControl.Rotation + weaponGyroscopeRotation.DeltaAngleOfHullRotation), -num4, num4);
                                effectiveControl = (Mathf.Abs(num4) > 0.001f) ? (f / num4) : 0f;
                            }
                            else
                            {
                                f = Mathf.Clamp(mouseRotationCumulativeHorizontalAngle, -num4, num4);
                                effectiveControl = (Mathf.Abs(num4) > 0.001f) ? (f / num4) : 0f;
                            }
                            mouseRotationCumulativeHorizontalAngle -= f;
                        }
                        f -= weaponGyroscopeRotation.DeltaAngleOfHullRotation;
                    }
                    else if (!weaponRotationControl.BlockRotate)
                    {
                        if (!weaponRotationControl.Centering)
                        {
                            f = Mathf.Clamp(mouseRotationCumulativeHorizontalAngle - weaponGyroscopeRotation.DeltaAngleOfHullRotation, -num4, num4);
                            effectiveControl = (Mathf.Abs(num4) > 0.001f) ? (f / num4) : 0f;
                        }
                        else
                        {
                            f = effectiveControl * num4;
                            float num9 = MathUtil.ClampAngle180(weaponRotationControl.Rotation);
                            if (Mathf.Abs(num9) < Mathf.Abs(f))
                            {
                                f = -num9;
                                effectiveControl = (Mathf.Abs(num4) > 0.001f) ? (f / num4) : 0f;
                            }
                        }
                        mouseRotationCumulativeHorizontalAngle = (mouseRotationCumulativeHorizontalAngle - f) - weaponGyroscopeRotation.DeltaAngleOfHullRotation;
                    }
                    if (weaponRotationControl.BlockRotate)
                    {
                        effectiveControl = 0f;
                    }
                }
                if (flag2)
                {
                    flag2 = false;
                }
                else if (Math.Sign(weaponRotationControl.PrevDeltaRotaion) != Math.Sign(f))
                {
                    weaponRotationControl.Speed = 0f;
                    flag2 = true;
                }
                if (!flag2)
                {
                    float num11;
                    weaponRotationControl.MouseRotationCumulativeHorizontalAngle = mouseRotationCumulativeHorizontalAngle;
                    weaponRotationControl.EffectiveControl = effectiveControl;
                    weaponRotationControl.PrevDeltaRotaion = f;
                    float rotation = weaponRotationControl.Rotation;
                    if (!weaponRotationControl.Centering || ((rotation >= -f) && ((360f - rotation) >= f)))
                    {
                        num11 = this.ClampAngle(rotation + f);
                    }
                    else
                    {
                        weaponRotationControl.Centering = false;
                        num11 = 0f;
                    }
                    weapon.weaponInstance.WeaponInstance.transform.SetLocalRotationSafe(Quaternion.AngleAxis(num11, Vector3.up));
                    weapon.weaponInstance.WeaponInstance.transform.localPosition = Vector3.zero;
                    weaponRotationControl.Rotation = num11;
                    weaponRotationControl.EffectiveControl = Mathf.Round(weaponRotationControl.EffectiveControl);
                    if (!Mathf.Approximately(weaponRotationControl.PrevEffectiveControl, weaponRotationControl.EffectiveControl) && ((PreciseTime.Time - weaponRotationControl.PrevControlChangedTime) > 0.1))
                    {
                        weaponRotationControl.PrevControlChangedTime = PreciseTime.Time;
                        weaponRotationControl.PrevEffectiveControl = weaponRotationControl.EffectiveControl;
                        base.ScheduleEvent<WeaponRotationControlChangedEvent>(weapon);
                    }
                    return;
                }
            }
        }

        public class MovableRemoteTankNode : Node
        {
            public RemoteTankComponent remoteTank;
            public TankGroupComponent tankGroup;
            public TankMovableComponent tankMovable;
        }

        public class SelfBattleUser : Node
        {
            public MouseControlStateHolderComponent mouseControlStateHolder;
            public SelfBattleUserComponent selfBattleUser;
        }

        public class SelfTankNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankMovableComponent tankMovable;
            public SelfTankComponent selfTank;
        }

        public class ShaftWeaponNode : WeaponRotationSystem.WeaponNode
        {
            public ShaftComponent shaft;
        }

        [Not(typeof(ShaftComponent))]
        public class SimpleWeaponNode : WeaponRotationSystem.WeaponNode
        {
        }

        public class TankIncarnationNode : Node
        {
            public TankIncarnationComponent tankIncarnation;
            public TankGroupComponent tankGroup;
        }

        public class WeaponNode : Node
        {
            public TankGroupComponent tankGroup;
            public WeaponInstanceComponent weaponInstance;
            public WeaponRotationControlComponent weaponRotationControl;
            public WeaponRotationComponent weaponRotation;
            public WeaponGyroscopeRotationComponent weaponGyroscopeRotation;
        }
    }
}

