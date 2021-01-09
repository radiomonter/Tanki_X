namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-3596341255560623830L)]
    public class TimeLimitComponent : Component
    {
        public long TimeLimitSec { get; set; }

        public long WarmingUpTimeLimitSec { get; set; }
    }
}

