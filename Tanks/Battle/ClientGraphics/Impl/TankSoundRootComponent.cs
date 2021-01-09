namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class TankSoundRootComponent : Component
    {
        public TankSoundRootComponent()
        {
        }

        public TankSoundRootComponent(Transform soundRootTransform)
        {
            this.SoundRootTransform = soundRootTransform;
        }

        public Transform SoundRootTransform { get; set; }
    }
}

