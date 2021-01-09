namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x288a97e4675106d2L)]
    public class FlagDropEvent : FlagEvent
    {
        public bool IsUserAction { get; set; }
    }
}

