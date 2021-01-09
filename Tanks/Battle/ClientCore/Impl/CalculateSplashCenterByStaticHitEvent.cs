namespace Tanks.Battle.ClientCore.Impl
{
    using System;

    public class CalculateSplashCenterByStaticHitEvent : CalculateSplashCenterEvent
    {
        public CalculateSplashCenterByStaticHitEvent()
        {
        }

        public CalculateSplashCenterByStaticHitEvent(SplashHitData splashHit) : base(splashHit)
        {
        }
    }
}

