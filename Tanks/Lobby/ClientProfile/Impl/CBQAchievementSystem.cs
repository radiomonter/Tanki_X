namespace Tanks.Lobby.ClientProfile.Impl
{
    using Lobby.ClientUserProfile.API;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientEntrance.API;

    public class CBQAchievementSystem : ECSSystem
    {
        [OnEventFire]
        public void ShowCBQBadge(NodeAddedEvent e, SingleNode<HomeScreenComponent> homeScreen, CBQUserNode selfUser)
        {
            homeScreen.component.CbqBadge.SetActive(true);
        }

        public class CBQUserNode : Node
        {
            public SelfUserComponent selfUser;
            public UserUidComponent userUid;
            public UserGroupComponent userGroup;
            public ClosedBetaQuestAchievementComponent closedBetaQuestAchievement;
        }
    }
}

