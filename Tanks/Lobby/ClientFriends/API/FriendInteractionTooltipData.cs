namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class FriendInteractionTooltipData
    {
        public Entity FriendEntity { get; set; }

        public bool ShowRemoveButton { get; set; }

        public bool ShowEnterAsSpectatorButton { get; set; }

        public bool ShowInviteToSquadButton { get; set; }

        public bool ActiveShowInviteToSquadButton { get; set; }

        public bool ShowRequestToSquadButton { get; set; }

        public bool ShowChatButton { get; set; }
    }
}

