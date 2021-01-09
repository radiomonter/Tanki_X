namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class StreamWeaponHitFeedbackReadyComponent : Component
    {
        public StreamWeaponHitFeedbackReadyComponent(Tanks.Battle.ClientGraphics.Impl.SoundController soundController)
        {
            this.SoundController = soundController;
        }

        public Tanks.Battle.ClientGraphics.Impl.SoundController SoundController { get; set; }
    }
}

