namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class HitFeedbackSoundsPlayingSettingsComponent : Component
    {
        public float Delay { get; set; }

        public bool RemoveOnKillSound { get; set; }

        public float Volume { get; set; }
    }
}

