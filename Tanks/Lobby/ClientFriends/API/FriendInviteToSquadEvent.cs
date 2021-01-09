namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientUserProfile.API;

    [Shared, SerialVersionUID(0x15f0b7285a3L)]
    public class FriendInviteToSquadEvent : Event
    {
        public FriendInviteToSquadEvent(long userId, Tanks.Lobby.ClientUserProfile.API.InteractionSource interactionSource, long sourceId)
        {
            this.UserId = userId;
            this.InteractionSource = interactionSource;
            this.SourceId = sourceId;
        }

        public long UserId { get; set; }

        public Tanks.Lobby.ClientUserProfile.API.InteractionSource InteractionSource { get; set; }

        public long SourceId { get; set; }
    }
}

