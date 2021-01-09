namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class IsisHitFeedbackReadyComponent : Component
    {
        public IsisHitFeedbackReadyComponent(SoundController healingSoundController, SoundController attackSoundController)
        {
            this.HealingSoundController = healingSoundController;
            this.AttackSoundController = attackSoundController;
        }

        public SoundController HealingSoundController { get; set; }

        public SoundController AttackSoundController { get; set; }
    }
}

