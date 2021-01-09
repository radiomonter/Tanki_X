namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15e0dca9efbL)]
    public class ElevatedAccessUserDropSupplyEvent : Event
    {
        public Tanks.Battle.ClientCore.Impl.BonusType BonusType { get; set; }
    }
}

