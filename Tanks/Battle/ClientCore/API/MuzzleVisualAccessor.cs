namespace Tanks.Battle.ClientCore.API
{
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    [StructLayout(LayoutKind.Sequential)]
    public struct MuzzleVisualAccessor
    {
        private MuzzlePointComponent component;
        public MuzzleVisualAccessor(MuzzlePointComponent component)
        {
            this.component = component;
        }

        public int GetCurrentIndex() => 
            this.component.CurrentIndex;

        public Vector3 GetWorldPosition() => 
            this.component.Current.position;

        public Vector3 GetBarrelOriginWorld()
        {
            Vector3 localPosition = this.component.Current.localPosition;
            localPosition.z = 0f;
            return this.component.Current.parent.TransformPoint(localPosition);
        }

        public unsafe Vector3 GetWorldPositionShiftDirectionBarrel(float shiftValue)
        {
            Vector3 localPosition = this.component.Current.localPosition;
            Vector3* vectorPtr1 = &localPosition;
            vectorPtr1->z *= shiftValue;
            return this.component.Current.parent.TransformPoint(localPosition);
        }

        public Vector3 GetWorldMiddlePosition()
        {
            Vector3 zero = Vector3.zero;
            foreach (Transform transform in this.component.Points)
            {
                zero += this.component.Current.position;
            }
            return (zero / ((float) this.component.Points.Length));
        }

        public Vector3 GetFireDirectionWorld() => 
            this.component.Current.forward;

        public Vector3 GetLeftDirectionWorld() => 
            -this.component.Current.right;

        public Vector3 GetUpDirectionWorld() => 
            this.component.Current.up;
    }
}

