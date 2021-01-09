namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x15e048a0d36L)]
    public class ForceFieldEffectComponent : Component
    {
        public bool OwnerTeamCanShootThrough { get; set; }
    }
}

