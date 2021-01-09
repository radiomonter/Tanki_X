namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ValidateSplashHitPointsEvent : Event
    {
        public ValidateSplashHitPointsEvent()
        {
        }

        public ValidateSplashHitPointsEvent(SplashHitData splashHit, List<GameObject> excludeObjects)
        {
            this.SplashHit = splashHit;
            this.excludeObjects = excludeObjects;
        }

        public SplashHitData SplashHit { get; set; }

        public List<GameObject> excludeObjects { get; set; }
    }
}

