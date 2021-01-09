namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d4d297c80a37f7L)]
    public class RageEffectComponent : Component
    {
        public int DecreaseCooldownPerKillMS { get; set; }
    }
}

