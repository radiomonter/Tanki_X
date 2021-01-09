namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class AcceleratedGearsInstanceComponent : Component
    {
        public AcceleratedGearsInstanceComponent(GameObject instance)
        {
            this.Instance = instance;
        }

        public GameObject Instance { get; set; }
    }
}

