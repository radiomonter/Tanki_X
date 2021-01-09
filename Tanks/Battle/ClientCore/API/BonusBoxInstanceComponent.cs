namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BonusBoxInstanceComponent : Component
    {
        public GameObject BonusBoxInstance { get; set; }

        public bool Removed { get; set; }
    }
}

