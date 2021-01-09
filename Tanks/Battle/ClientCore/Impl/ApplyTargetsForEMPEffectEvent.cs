namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d46aca2bde76d9L)]
    public class ApplyTargetsForEMPEffectEvent : Event
    {
        public Entity[] Targets { get; set; }
    }
}

