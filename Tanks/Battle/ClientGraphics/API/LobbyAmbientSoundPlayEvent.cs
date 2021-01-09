namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class LobbyAmbientSoundPlayEvent : Event
    {
        public LobbyAmbientSoundPlayEvent(bool hymnMode)
        {
            this.HymnMode = hymnMode;
        }

        public bool HymnMode { get; set; }
    }
}

