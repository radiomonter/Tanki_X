namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BonusRegionInstanceComponent : Component
    {
        public BonusRegionInstanceComponent()
        {
        }

        public BonusRegionInstanceComponent(GameObject bonusRegionInstance)
        {
            this.BonusRegionInstance = bonusRegionInstance;
        }

        public GameObject BonusRegionInstance { get; set; }
    }
}

