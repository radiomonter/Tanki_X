namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class PreloadedModuleEffectsComponent : Component
    {
        public PreloadedModuleEffectsComponent(Dictionary<string, GameObject> preloadedEffects)
        {
            this.PreloadedEffects = preloadedEffects;
        }

        public Dictionary<string, GameObject> PreloadedEffects { get; set; }
    }
}

