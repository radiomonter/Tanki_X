namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15e0f34be5fL)]
    public class ElevatedAccessUserRunCommandEvent : Event
    {
        public string Command { get; set; }
    }
}

