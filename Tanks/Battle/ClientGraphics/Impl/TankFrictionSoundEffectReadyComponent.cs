namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class TankFrictionSoundEffectReadyComponent : Component
    {
        public Tanks.Battle.ClientGraphics.Impl.TankFrictionSoundBehaviour TankFrictionSoundBehaviour { get; set; }

        public Tanks.Battle.ClientGraphics.Impl.TankFrictionCollideSoundBehaviour TankFrictionCollideSoundBehaviour { get; set; }
    }
}

