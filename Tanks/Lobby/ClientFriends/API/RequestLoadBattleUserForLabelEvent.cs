namespace Tanks.Lobby.ClientFriends.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x154c7d937ebL)]
    public class RequestLoadBattleUserForLabelEvent : Event
    {
        public RequestLoadBattleUserForLabelEvent(Entity sessionEntity)
        {
            this.SessionEntity = sessionEntity;
        }

        public Entity SessionEntity { get; set; }
    }
}

