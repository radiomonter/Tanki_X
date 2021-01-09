namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientGraphics.API;

    public class AmbientMapSoundEffectComponent : Component
    {
        public AmbientMapSoundEffectComponent()
        {
        }

        public AmbientMapSoundEffectComponent(AmbientSoundFilter ambientMapSound)
        {
            this.AmbientMapSound = ambientMapSound;
        }

        public AmbientSoundFilter AmbientMapSound { get; set; }
    }
}

