namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x160d9f6096aL)]
    public class ElevatedAccessUserAddKillsEvent : Event
    {
        public int Count { get; set; }
    }
}

