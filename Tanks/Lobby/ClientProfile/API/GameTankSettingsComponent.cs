namespace Tanks.Lobby.ClientProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class GameTankSettingsComponent : Component
    {
        public bool MovementControlsInverted { get; set; }

        public bool DamageInfoEnabled { get; set; }

        public bool HealthFeedbackEnabled { get; set; }

        public bool SelfTargetHitFeedbackEnabled { get; set; }
    }
}

