namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x156f8ef6f29L)]
    public class XCrystalsPackComponent : Component
    {
        public long Amount { get; set; }

        public long Bonus { get; set; }
    }
}

