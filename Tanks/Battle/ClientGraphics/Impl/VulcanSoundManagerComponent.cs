namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class VulcanSoundManagerComponent : Component
    {
        public VulcanSoundManagerComponent()
        {
            this.SoundsWithDelay = new Dictionary<AudioSource, float>();
        }

        public AudioSource CurrentSound { get; set; }

        public Dictionary<AudioSource, float> SoundsWithDelay { get; set; }
    }
}

