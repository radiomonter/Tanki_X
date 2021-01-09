namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BrokenBonusBoxInstanceComponent : Component
    {
        public BrokenBonusBoxInstanceComponent(GameObject instance)
        {
            this.Instance = instance;
        }

        public GameObject Instance { get; set; }
    }
}

