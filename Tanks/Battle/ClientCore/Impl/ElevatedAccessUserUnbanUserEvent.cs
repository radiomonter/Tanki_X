namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x160367be221L)]
    public class ElevatedAccessUserUnbanUserEvent : Event
    {
        public string Uid { get; set; }
    }
}

