namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientUserProfile.API;

    public class UserLabelLoadSystem : ECSSystem
    {
        [OnEventFire]
        public void AttachUser(NodeAddedEvent e, UserLabelLoadedNode userLabel)
        {
            userLabel.Entity.AddComponent(new UserGroupComponent(userLabel.userLabel.UserId));
        }

        public class UserLabelLoadedNode : Node
        {
            public UserLabelComponent userLabel;
            public LoadUserComponent loadUser;
            public UserLoadedComponent userLoaded;
        }
    }
}

