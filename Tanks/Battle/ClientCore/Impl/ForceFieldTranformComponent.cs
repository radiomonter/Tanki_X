namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x15e9f083c10L), Shared]
    public class ForceFieldTranformComponent : Component
    {
        public Tanks.Battle.ClientCore.Impl.Movement Movement { get; set; }
    }
}

