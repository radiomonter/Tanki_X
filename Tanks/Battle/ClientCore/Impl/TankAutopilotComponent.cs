namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x151d35e20e6L), Shared]
    public class TankAutopilotComponent : Component
    {
        [ProtocolOptional]
        public Entity Session { get; set; }

        public int Version { get; set; }
    }
}

