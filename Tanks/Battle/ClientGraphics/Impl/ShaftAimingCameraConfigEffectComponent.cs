namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class ShaftAimingCameraConfigEffectComponent : MonoBehaviour, Component
    {
        [SerializeField]
        private float activationStateTargetFOV = 40f;
        [SerializeField]
        private float workingStateMinFOV = 23f;
        [SerializeField]
        private float recoveringFOVSpeed = 30f;

        public float RecoveringFovSpeed
        {
            get => 
                this.recoveringFOVSpeed;
            set => 
                this.recoveringFOVSpeed = value;
        }

        public float ActivationStateTargetFov
        {
            get => 
                this.activationStateTargetFOV;
            set => 
                this.activationStateTargetFOV = value;
        }

        public float WorkingStateMinFov
        {
            get => 
                this.workingStateMinFOV;
            set => 
                this.workingStateMinFOV = value;
        }
    }
}

