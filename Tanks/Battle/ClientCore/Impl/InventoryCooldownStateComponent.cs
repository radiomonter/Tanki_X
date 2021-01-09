namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15a22608050L)]
    public class InventoryCooldownStateComponent : Component
    {
        public int CooldownTime { get; set; }

        public Date CooldownStartTime { get; set; }
    }
}

