namespace Tanks.Lobby.ClientFriends.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientFriends.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientUserProfile.API;

    public class DisplayProfileScreenHeaderSystem : ECSSystem
    {
        [OnEventFire]
        public void SetFriendProfileHeader(NodeAddedEvent e, ProfileScreenWithUserGroupNode profileScreen, [Context, JoinByUser] FriendUserNode friendUser)
        {
            SetScreenHeaderEvent eventInstance = new SetScreenHeaderEvent();
            eventInstance.Immediate(profileScreen.profileScreenLocalization.FriendsProfileHeaderText);
            base.ScheduleEvent(eventInstance, profileScreen);
        }

        [OnEventFire]
        public void SetMyProfileHeader(NodeAddedEvent e, ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] SelfUserNode selfUser)
        {
            profileScreen.Entity.AddComponent(new ScreenHeaderTextComponent(profileScreen.profileScreenLocalization.MyProfileHeaderText));
        }

        [OnEventFire]
        public void SetNotFriendProfileHeader(NodeAddedEvent e, ProfileScreenWithUserGroupNode profileScreen, [JoinByUser] NotFriendUserNode notFriendUser)
        {
            new SetScreenHeaderEvent().Immediate(profileScreen.profileScreenLocalization.ProfileHeaderText);
            base.ScheduleEvent<SetScreenHeaderEvent>(profileScreen);
        }

        [OnEventFire]
        public void SetNotFriendProfileHeader(NodeRemoveEvent e, FriendUserNode friendUser, [JoinByUser] ProfileScreenWithUserGroupNode profileScreen)
        {
            SetScreenHeaderEvent eventInstance = new SetScreenHeaderEvent();
            eventInstance.Immediate(profileScreen.profileScreenLocalization.ProfileHeaderText);
            base.ScheduleEvent(eventInstance, profileScreen);
        }

        public class FriendUserNode : Node
        {
            public AcceptedFriendComponent acceptedFriend;
            public UserGroupComponent userGroup;
        }

        [Not(typeof(AcceptedFriendComponent)), Not(typeof(SelfUserComponent))]
        public class NotFriendUserNode : Node
        {
            public UserComponent user;
            public UserGroupComponent userGroup;
        }

        public class ProfileScreenWithUserGroupNode : Node
        {
            public ProfileScreenComponent profileScreen;
            public ProfileScreenLocalizationComponent profileScreenLocalization;
            public UserGroupComponent userGroup;
            public ActiveScreenComponent activeScreen;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserGroupComponent userGroup;
        }
    }
}

