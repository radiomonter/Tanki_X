namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class GetInstanceFromPoolEvent : Event
    {
        public GameObject Prefab;
        public Transform Instance;
        public float AutoRecycleTime = -1f;
    }
}

