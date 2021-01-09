namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class WaterSettingsComponent : Component
    {
        public bool HasReflection { get; set; }

        public bool EdgeBlend { get; set; }
    }
}

