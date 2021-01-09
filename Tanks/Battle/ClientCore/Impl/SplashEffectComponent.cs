namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1671c090950L)]
    public class SplashEffectComponent : Component
    {
        public bool CanTargetTeammates { get; set; }
    }
}

