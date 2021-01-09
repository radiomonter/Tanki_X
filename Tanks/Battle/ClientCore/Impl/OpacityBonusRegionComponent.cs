namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class OpacityBonusRegionComponent : Component
    {
        public float Opacity { get; set; }
    }
}

