namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientGraphics.API;

    public class VulcanShootingCameraShakerInstanceComponent : Component
    {
        public VulcanShootingCameraShakerInstanceComponent(CameraShakeInstance instance, float fadeOutTime)
        {
            this.Instance = instance;
            this.FadeOutTime = fadeOutTime;
        }

        public CameraShakeInstance Instance { get; set; }

        public float FadeOutTime { get; set; }
    }
}

