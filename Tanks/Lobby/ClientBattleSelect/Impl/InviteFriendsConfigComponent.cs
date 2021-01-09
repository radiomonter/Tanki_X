namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class InviteFriendsConfigComponent : Component
    {
        public float InviteSentNotificationDuration { get; set; }
    }
}

