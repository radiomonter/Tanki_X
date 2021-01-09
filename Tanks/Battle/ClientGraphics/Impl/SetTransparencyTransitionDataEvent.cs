namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SetTransparencyTransitionDataEvent : Event
    {
        public SetTransparencyTransitionDataEvent(float originAlpha, float targetAlpha, float transparencyTransitionTime)
        {
            this.OriginAlpha = originAlpha;
            this.TargetAlpha = targetAlpha;
            this.TransparencyTransitionTime = transparencyTransitionTime;
        }

        public float OriginAlpha { get; set; }

        public float TargetAlpha { get; set; }

        public float TransparencyTransitionTime { get; set; }
    }
}

