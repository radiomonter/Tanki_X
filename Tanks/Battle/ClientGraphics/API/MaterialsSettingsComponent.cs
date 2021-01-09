namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class MaterialsSettingsComponent : Component
    {
        public int GlobalShadersMaximumLOD { get; set; }
    }
}

