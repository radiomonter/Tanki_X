namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1609777091eL)]
    public class ElevatedAccessUserAddScoreEvent : Event
    {
        public int Count { get; set; }
    }
}

