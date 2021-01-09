namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CalculateSplashTargetsWithCenterEvent : Event
    {
        public CalculateSplashTargetsWithCenterEvent()
        {
        }

        public CalculateSplashTargetsWithCenterEvent(SplashHitData splashHit)
        {
            this.SplashHit = splashHit;
        }

        public SplashHitData SplashHit { get; set; }
    }
}

