namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    public class IdleKickCheckLastTimeComponent : Component
    {
        public Date CheckLastTime { get; set; }
    }
}

