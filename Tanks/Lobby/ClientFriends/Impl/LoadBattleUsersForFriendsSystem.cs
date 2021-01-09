namespace Tanks.Lobby.ClientFriends.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientFriends.API;

    public class LoadBattleUsersForFriendsSystem : ECSSystem
    {
        public class FriendInBattleNode : Node
        {
            public AcceptedFriendComponent acceptedFriend;
            public UserGroupComponent userGroup;
            public BattleGroupComponent battleGroup;
        }
    }
}

