namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class MapEffectInstanceComponent : Component
    {
        public MapEffectInstanceComponent(GameObject instance)
        {
            this.Instance = instance;
        }

        public GameObject Instance { get; set; }
    }
}

