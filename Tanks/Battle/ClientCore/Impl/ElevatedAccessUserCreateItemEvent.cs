namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x160974f7dbcL)]
    public class ElevatedAccessUserCreateItemEvent : Event
    {
        public long Count { get; set; }

        public long ItemId { get; set; }
    }
}

