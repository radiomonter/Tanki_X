namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class JoinScreenElementToUserSystem : ECSSystem
    {
        [OnEventFire]
        public void BreakJoin(NodeRemoveEvent e, UserNode user, [Combine, JoinByUser] JoinScreenElementToUserGroupNode screenElement)
        {
            user.userGroup.Detach(screenElement.Entity);
        }

        [OnEventFire]
        public void Join(NodeAddedEvent e, [Combine] JoinScreenElementToUserGroupNode screenElement, [Context, JoinByScreen] ActiveScreenWithUserGroupNode activeScreen, [JoinByUser] UserNode user)
        {
            user.userGroup.Attach(screenElement.Entity);
        }

        public class ActiveScreenWithUserGroupNode : Node
        {
            public ActiveScreenComponent activeScreen;
            public ScreenGroupComponent screenGroup;
            public UserGroupComponent userGroup;
        }

        public class JoinScreenElementToUserGroupNode : Node
        {
            public JoinScreenElementToUserComponent joinScreenElementToUser;
            public ScreenGroupComponent screenGroup;
        }

        public class UserNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
        }
    }
}

