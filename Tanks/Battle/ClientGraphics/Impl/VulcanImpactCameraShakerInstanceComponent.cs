namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class VulcanImpactCameraShakerInstanceComponent : Component
    {
        public VulcanImpactCameraShakerInstanceComponent(float fadeOutTime)
        {
            this.Init(fadeOutTime);
        }

        public void Init(float fadeOutTime)
        {
            this.FadeOutTime = fadeOutTime;
            this.ImpactDataChanged = false;
            this.ImpactDirection = Vector3.zero;
            this.WeakeningCoeff = -1f;
        }

        public Tanks.Battle.ClientGraphics.API.CameraShakeInstance CameraShakeInstance { get; set; }

        public float FadeOutTime { get; set; }

        public Vector3 ImpactDirection { get; set; }

        public float WeakeningCoeff { get; set; }

        public bool ImpactDataChanged { get; set; }
    }
}

