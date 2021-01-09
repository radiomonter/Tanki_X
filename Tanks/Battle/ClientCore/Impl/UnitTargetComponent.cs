namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x15a17a2bf47L), Shared]
    public class UnitTargetComponent : Component
    {
        public Entity Target { get; set; }

        public Entity TargetIncarnation { get; set; }
    }
}

