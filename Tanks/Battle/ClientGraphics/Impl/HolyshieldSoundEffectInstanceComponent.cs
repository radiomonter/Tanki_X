namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class HolyshieldSoundEffectInstanceComponent : Component
    {
        public HolyshieldSoundEffectInstanceComponent(SoundController instance)
        {
            this.Instance = instance;
        }

        public SoundController Instance { get; set; }
    }
}

