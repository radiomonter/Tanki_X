namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    [Shared, SerialVersionUID(0x8d4f5ef9b895a10L)]
    public class ElevatedAccessUserDropSupplyGoldEvent : Event
    {
        public Tanks.Battle.ClientCore.API.GoldType GoldType { get; set; }
    }
}

