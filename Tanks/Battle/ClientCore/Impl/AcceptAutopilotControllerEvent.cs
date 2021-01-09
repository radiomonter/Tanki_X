namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x151d35e2cedL), Shared]
    public class AcceptAutopilotControllerEvent : Event
    {
        public int Version { get; set; }
    }
}

