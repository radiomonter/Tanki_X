namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Tanks.Lobby.ClientEntrance.API;

    public class RequestToSquadSystem : ECSSystem
    {
        public class SelfUserNode : Node
        {
            public UserComponent user;
            public SelfUserComponent selfUser;
        }
    }
}

