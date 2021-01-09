namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4df17d419b4bdL)]
    public class InventoryLimitBundleEffectsComponent : Component
    {
        public int BundleEffectLimit { get; set; }
    }
}

