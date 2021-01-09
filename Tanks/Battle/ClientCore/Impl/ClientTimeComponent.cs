namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ClientTimeComponent : Component
    {
        public long PingServerTime { get; set; }

        public float PingClientRealTime { get; set; }
    }
}

