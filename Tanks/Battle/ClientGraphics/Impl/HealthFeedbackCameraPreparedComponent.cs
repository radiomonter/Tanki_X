namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class HealthFeedbackCameraPreparedComponent : Component
    {
        public HealthFeedbackCameraPreparedComponent(Tanks.Battle.ClientGraphics.Impl.HealthFeedbackPostEffect healthFeedbackPostEffect)
        {
            this.HealthFeedbackPostEffect = healthFeedbackPostEffect;
        }

        public float TargetIntensity { get; set; }

        public Tanks.Battle.ClientGraphics.Impl.HealthFeedbackPostEffect HealthFeedbackPostEffect { get; set; }
    }
}

