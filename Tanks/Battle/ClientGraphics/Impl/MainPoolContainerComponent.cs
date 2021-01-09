namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class MainPoolContainerComponent : Component
    {
        public Transform MainContainerTransform;
        public readonly Dictionary<GameObject, PoolContainer> PrefabToPoolDict = new Dictionary<GameObject, PoolContainer>();
    }
}

