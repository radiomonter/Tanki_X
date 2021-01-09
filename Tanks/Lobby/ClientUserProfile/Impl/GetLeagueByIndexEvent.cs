namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class GetLeagueByIndexEvent : Event
    {
        public int index;
        public Entity leagueEntity;

        public GetLeagueByIndexEvent(int index)
        {
            this.index = index;
            this.leagueEntity = null;
        }
    }
}

