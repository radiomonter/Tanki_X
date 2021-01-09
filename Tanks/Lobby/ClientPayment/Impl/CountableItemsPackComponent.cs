namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15f0f637e83L)]
    public class CountableItemsPackComponent : Component
    {
        public Dictionary<long, int> Pack { get; set; }
    }
}

