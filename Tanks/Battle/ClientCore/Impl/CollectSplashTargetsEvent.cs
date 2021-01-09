namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CollectSplashTargetsEvent : Event
    {
        public CollectSplashTargetsEvent()
        {
        }

        public CollectSplashTargetsEvent(SplashHitData splashHit)
        {
            this.SplashHit = splashHit;
        }

        public SplashHitData SplashHit { get; set; }
    }
}

