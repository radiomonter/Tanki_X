namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CheckForShowInviteToSquadEvent : Event
    {
        public bool ShowInviteToSquadButton { get; set; }

        public bool ActiveInviteToSquadButton { get; set; }

        public bool ShowRequestToInviteToSquadButton { get; set; }
    }
}

