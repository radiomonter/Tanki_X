namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientGraphics.API;

    public class PlayingMelodyRoundRestartListenerComponent : Component
    {
        public PlayingMelodyRoundRestartListenerComponent(AmbientSoundFilter melody)
        {
            this.Melody = melody;
        }

        public AmbientSoundFilter Melody { get; set; }
    }
}

