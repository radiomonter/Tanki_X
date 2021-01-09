namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class IdleKickConfigComponent : Component
    {
        public int IdleKickTimeSec { get; set; }

        public int IdleWarningTimeSec { get; set; }

        public int CheckPeriodicTimeSec { get; set; }
    }
}

