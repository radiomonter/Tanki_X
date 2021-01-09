namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ReticleTemplateComponent : Component
    {
        public TemplateDescription Template { get; set; }

        public string ConfigPath { get; set; }
    }
}

