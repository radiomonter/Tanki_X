namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15f0f5eeaf4L)]
    public class CrystalsPackComponent : Component
    {
        public long Amount { get; set; }

        public long Bonus { get; set; }

        [ProtocolTransient]
        public long Total =>
            this.Amount + this.Bonus;
    }
}

