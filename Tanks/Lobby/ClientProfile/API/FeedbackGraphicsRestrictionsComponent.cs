namespace Tanks.Lobby.ClientProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class FeedbackGraphicsRestrictionsComponent : Component
    {
        public bool HealthFeedbackAllowed { get; set; }

        public bool SelfTargetHitFeedbackAllowed { get; set; }
    }
}

