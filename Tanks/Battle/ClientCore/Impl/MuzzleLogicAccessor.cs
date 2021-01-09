namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using System.Runtime.InteropServices;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct MuzzleLogicAccessor
    {
        private MuzzlePointComponent muzzleComponent;
        private Transform weaponTransform;
        public MuzzleLogicAccessor(MuzzlePointComponent muzzleComponent, WeaponInstanceComponent weaponInstanceComponent)
        {
            this.muzzleComponent = muzzleComponent;
            this.weaponTransform = weaponInstanceComponent.WeaponInstance.transform;
        }

        public int GetCurrentIndex() => 
            this.muzzleComponent.CurrentIndex;

        public Vector3 GetWorldPosition() => 
            this.weaponTransform.TransformPoint(this.muzzleComponent.Current.localPosition);

        public Vector3 GetBarrelOriginWorld()
        {
            Vector3 localPosition = this.muzzleComponent.Current.localPosition;
            localPosition.z = 0f;
            return this.weaponTransform.TransformPoint(localPosition);
        }

        public unsafe Vector3 GetWorldPositionShiftDirectionBarrel(float shiftValue)
        {
            Vector3 localPosition = this.muzzleComponent.Current.localPosition;
            Vector3* vectorPtr1 = &localPosition;
            vectorPtr1->z *= shiftValue;
            return this.weaponTransform.TransformPoint(localPosition);
        }

        public Vector3 GetWorldMiddlePosition()
        {
            Vector3 zero = Vector3.zero;
            foreach (Transform transform in this.muzzleComponent.Points)
            {
                zero += this.muzzleComponent.Current.localPosition;
            }
            return this.weaponTransform.TransformPoint(zero / ((float) this.muzzleComponent.Points.Length));
        }

        public Vector3 GetFireDirectionWorld() => 
            this.weaponTransform.forward;

        public Vector3 GetLeftDirectionWorld() => 
            -this.weaponTransform.right;

        public Vector3 GetUpDirectionWorld() => 
            this.weaponTransform.up;
    }
}

