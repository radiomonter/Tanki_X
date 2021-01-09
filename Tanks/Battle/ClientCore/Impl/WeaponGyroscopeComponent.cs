namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class WeaponGyroscopeComponent : Component
    {
        public float ForceMult { get; set; }
    }
}

