namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TankJumpSoundComponent : Component
    {
        public TankJumpSoundComponent()
        {
        }

        public TankJumpSoundComponent(AudioSource sound)
        {
            this.Sound = sound;
        }

        public AudioSource Sound { get; set; }
    }
}

