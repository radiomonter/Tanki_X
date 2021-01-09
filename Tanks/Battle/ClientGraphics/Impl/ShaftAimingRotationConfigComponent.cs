namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class ShaftAimingRotationConfigComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private float aimingOffsetClipping = 35f;
        [SerializeField]
        private float maxAimingCameraOffset = 30f;

        public float AimingOffsetClipping =>
            this.aimingOffsetClipping;

        public float MaxAimingCameraOffset =>
            this.maxAimingCameraOffset;
    }
}

