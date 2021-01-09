namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x74dc56486f818cfcL), Shared]
    public class HealthConfigComponent : Component
    {
        public float BaseHealth { get; set; }
    }
}

