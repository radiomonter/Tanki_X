namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class WeaponGyroscopeRotationSystem : ECSSystem
    {
        private float CalculateDeltaAngleOfHullRotationAroundUpAxis(Vector3 prevForward, Vector3 nextForward, Vector3 prevUp, Vector3 nextUp)
        {
            if ((prevForward == nextForward) && (prevUp == nextUp))
            {
                return 0f;
            }
            Vector3 rhs = (Vector3) (Quaternion.FromToRotation(prevUp, nextUp) * prevForward);
            Vector3 vector2 = Vector3.Cross(nextUp, rhs);
            return (Mathf.Sign(Vector3.Dot(nextForward, vector2)) * Vector3.Angle(nextForward, rhs));
        }

        [OnEventComplete]
        public void RotateWeapon(WeaponRotationUpdateGyroscopeEvent e, WeaponGyroscopeNode weapon, [JoinByTank] TankNode tank, [JoinByTank] Optional<SingleNode<VulcanWeaponComponent>> vulkanWeapon, [JoinAll] SelfBattleUser selfBattleUser)
        {
            WeaponRotationControlComponent weaponRotationControl = weapon.weaponRotationControl;
            float gyroscopePower = weapon.weaponGyroscopeRotation.GyroscopePower;
            weaponRotationControl.ForceGyroscopeEnabled = vulkanWeapon.IsPresent() && (gyroscopePower > float.Epsilon);
            bool mouseControlEnable = selfBattleUser.mouseControlStateHolder.MouseControlEnable;
            WeaponGyroscopeRotationComponent weaponGyroscopeRotation = weapon.weaponGyroscopeRotation;
            weaponGyroscopeRotation.WeaponTurnCoeff = 1f;
            weaponGyroscopeRotation.DeltaAngleOfHullRotation = 0f;
            Vector3 forwardDir = weaponGyroscopeRotation.ForwardDir;
            Vector3 upDir = weaponGyroscopeRotation.UpDir;
            this.UpdateGyroscopeData(weapon, tank);
            Vector3 nextForward = weaponGyroscopeRotation.ForwardDir;
            Vector3 nextUp = weaponGyroscopeRotation.UpDir;
            if (weaponRotationControl.ForceGyroscopeEnabled || mouseControlEnable)
            {
                weaponGyroscopeRotation.DeltaAngleOfHullRotation = this.CalculateDeltaAngleOfHullRotationAroundUpAxis(forwardDir, nextForward, upDir, nextUp);
                float max = tank.speed.TurnSpeed * e.DeltaTime;
                weaponGyroscopeRotation.DeltaAngleOfHullRotation = Mathf.Clamp(weaponGyroscopeRotation.DeltaAngleOfHullRotation, -max, max);
                if (weaponRotationControl.ForceGyroscopeEnabled)
                {
                    if (mouseControlEnable)
                    {
                        weaponRotationControl.MouseRotationCumulativeHorizontalAngle -= weaponGyroscopeRotation.DeltaAngleOfHullRotation * (1f - gyroscopePower);
                    }
                    float weaponTurnDecelerationCoeff = vulkanWeapon.Get().component.WeaponTurnDecelerationCoeff;
                    weaponGyroscopeRotation.WeaponTurnCoeff = weaponTurnDecelerationCoeff + ((1f - gyroscopePower) * (1f - weaponTurnDecelerationCoeff));
                    weaponGyroscopeRotation.DeltaAngleOfHullRotation *= gyroscopePower;
                }
            }
        }

        [OnEventFire]
        public void TakeOrientationWeapon(NodeAddedEvent evt, WeaponGyroscopeNode weaponGyroscope, [Context, JoinByTank] TankNode tank)
        {
            this.UpdateGyroscopeData(weaponGyroscope, tank);
        }

        private void UpdateGyroscopeData(WeaponGyroscopeNode weaponGyroscope, TankNode tank)
        {
            WeaponGyroscopeRotationComponent weaponGyroscopeRotation = weaponGyroscope.weaponGyroscopeRotation;
            Transform transform = tank.hullInstance.HullInstance.transform;
            weaponGyroscopeRotation.ForwardDir = transform.forward;
            weaponGyroscopeRotation.UpDir = transform.up;
        }

        public class SelfBattleUser : Node
        {
            public MouseControlStateHolderComponent mouseControlStateHolder;
            public SelfBattleUserComponent selfBattleUser;
        }

        public class TankNode : Node
        {
            public TankGroupComponent tankGroup;
            public TankMovableComponent tankMovable;
            public HullInstanceComponent hullInstance;
            public SpeedComponent speed;
        }

        public class WeaponGyroscopeNode : Node
        {
            public TankGroupComponent tankGroup;
            public WeaponInstanceComponent weaponInstance;
            public WeaponRotationControlComponent weaponRotationControl;
            public WeaponRotationComponent weaponRotation;
            public WeaponGyroscopeRotationComponent weaponGyroscopeRotation;
        }
    }
}

