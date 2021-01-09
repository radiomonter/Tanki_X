namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class SelfUserRankSoundEffectInstanceComponent : Component
    {
        public SelfUserRankSoundEffectInstanceComponent(AudioSource source)
        {
            this.Source = source;
        }

        public AudioSource Source { get; set; }
    }
}

