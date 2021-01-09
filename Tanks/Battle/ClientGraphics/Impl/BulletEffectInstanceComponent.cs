namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class BulletEffectInstanceComponent : Component
    {
        public GameObject Effect { get; set; }
    }
}

