namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class EarnedGameplayChestScoreComponent : Component
    {
        public long Earned { get; set; }
    }
}

