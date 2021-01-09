namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class LeagueEnterNotificationTextsComponent : Component
    {
        public string HeaderText { get; set; }

        public string Text { get; set; }
    }
}

