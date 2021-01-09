namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class CriticalEffectEvent : Event
    {
        public GameObject EffectPrefab { get; set; }

        public Vector3 LocalPosition { get; set; }
    }
}

