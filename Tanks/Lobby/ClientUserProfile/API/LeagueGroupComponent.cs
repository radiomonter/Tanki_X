namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0x15e038b7b3bL)]
    public class LeagueGroupComponent : GroupComponent
    {
        public LeagueGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public LeagueGroupComponent(long key) : base(key)
        {
        }
    }
}

