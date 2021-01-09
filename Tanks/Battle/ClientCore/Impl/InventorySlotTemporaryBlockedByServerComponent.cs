namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4d4e359816ad8L)]
    public class InventorySlotTemporaryBlockedByServerComponent : Component
    {
        public long BlockTimeMS { get; set; }

        public Date StartBlockTime { get; set; }
    }
}

