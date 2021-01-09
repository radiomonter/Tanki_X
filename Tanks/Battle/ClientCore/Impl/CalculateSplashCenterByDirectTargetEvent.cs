namespace Tanks.Battle.ClientCore.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CalculateSplashCenterByDirectTargetEvent : CalculateSplashCenterEvent
    {
        public CalculateSplashCenterByDirectTargetEvent()
        {
        }

        public CalculateSplashCenterByDirectTargetEvent(Vector3 directTargetLocalHitPoint)
        {
            this.DirectTargetLocalHitPoint = directTargetLocalHitPoint;
        }

        public CalculateSplashCenterByDirectTargetEvent(SplashHitData splashHit, Vector3 directTargetLocalHitPoint) : base(splashHit)
        {
            this.DirectTargetLocalHitPoint = directTargetLocalHitPoint;
        }

        public Vector3 DirectTargetLocalHitPoint { get; set; }
    }
}

