namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientUserProfile.API;
    using Tanks.Lobby.ClientUserProfile.Impl;

    public class ProfileStatsSectionUISystem : ECSSystem
    {
        [OnEventFire]
        public void SetLevelInfo(NodeAddedEvent e, ProfileStatsSectionUINode sectionUI, [JoinAll] UserStatisticsNode statistics)
        {
            GetUserLevelInfoEvent eventInstance = new GetUserLevelInfoEvent();
            base.ScheduleEvent(eventInstance, statistics);
            sectionUI.profileStatsSectionUI.SetRank(eventInstance.Info);
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }

        public class ProfileStatsSectionUINode : Node
        {
            public ProfileStatsSectionUIComponent profileStatsSectionUI;
        }

        public class UserStatisticsNode : Node
        {
            public SelfUserComponent selfUser;
            public UserStatisticsComponent userStatistics;
            public FavoriteEquipmentStatisticsComponent favoriteEquipmentStatistics;
            public KillsEquipmentStatisticsComponent killsEquipmentStatistics;
        }
    }
}

