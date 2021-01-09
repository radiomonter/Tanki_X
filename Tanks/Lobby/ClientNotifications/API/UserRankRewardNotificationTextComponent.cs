namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class UserRankRewardNotificationTextComponent : Component
    {
        public string RankHeaderText { get; set; }

        public string RewardLabelText { get; set; }
    }
}

