namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class AmbientZoneSoundEffectComponent : Component
    {
        public AmbientZoneSoundEffectComponent(Tanks.Battle.ClientGraphics.Impl.AmbientZoneSoundEffect ambientZoneSoundEffect)
        {
            this.AmbientZoneSoundEffect = ambientZoneSoundEffect;
        }

        public Tanks.Battle.ClientGraphics.Impl.AmbientZoneSoundEffect AmbientZoneSoundEffect { get; set; }
    }
}

