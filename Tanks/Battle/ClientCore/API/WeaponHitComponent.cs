namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class WeaponHitComponent : Component
    {
        public WeaponHitComponent(bool sendStaticHit, bool removeDuplicateTargets)
        {
            this.SendStaticHit = sendStaticHit;
            this.RemoveDuplicateTargets = removeDuplicateTargets;
        }

        public bool SendStaticHit { get; set; }

        public bool RemoveDuplicateTargets { get; set; }
    }
}

