namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientUserProfile.API;

    public class UserInteractionsData
    {
        public bool canRequestFrendship;
        public bool friendshipRequestWasSend;
        public bool isMuted;
        public bool isReported;
        public long userId;
        public Entity selfUserEntity;
        public InteractionSource interactionSource;
        public long sourceId;
        public string OtherUserName;
    }
}

