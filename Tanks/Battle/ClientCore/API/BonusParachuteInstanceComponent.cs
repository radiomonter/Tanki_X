namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BonusParachuteInstanceComponent : Component
    {
        public BonusParachuteInstanceComponent(GameObject bonusParachuteInstance)
        {
            this.BonusParachuteInstance = bonusParachuteInstance;
        }

        public GameObject BonusParachuteInstance { get; set; }
    }
}

