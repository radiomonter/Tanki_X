namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ShowBattleResultsScreenNotificationEvent : Event
    {
        public int Index { get; set; }

        public bool NotificationExist { get; set; }
    }
}

