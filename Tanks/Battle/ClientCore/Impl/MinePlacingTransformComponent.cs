namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class MinePlacingTransformComponent : Component
    {
        public RaycastHit PlacingData { get; set; }

        public bool HasPlacingTransform { get; set; }
    }
}

