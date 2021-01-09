namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CalculateSplashCenterEvent : Event
    {
        public CalculateSplashCenterEvent()
        {
        }

        public CalculateSplashCenterEvent(SplashHitData splashHit)
        {
            this.SplashHit = splashHit;
        }

        public SplashHitData SplashHit { get; set; }
    }
}

