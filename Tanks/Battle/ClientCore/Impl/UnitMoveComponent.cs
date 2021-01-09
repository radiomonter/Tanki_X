namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x159dfd8111bL), Shared]
    public class UnitMoveComponent : Component
    {
        public Tanks.Battle.ClientCore.Impl.Movement Movement { get; set; }
    }
}

