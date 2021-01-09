namespace Tanks.Lobby.ClientNotifications.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class IgnoreBattleResultScreenNotificationComponent : Component
    {
        public int ScreenPartIndex { get; set; }
    }
}

