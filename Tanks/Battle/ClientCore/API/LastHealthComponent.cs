namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class LastHealthComponent : Component
    {
        public float LastHealth { get; set; }
    }
}

