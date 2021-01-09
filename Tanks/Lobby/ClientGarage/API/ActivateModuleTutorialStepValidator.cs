namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using Tanks.Lobby.ClientUserProfile.API;

    public class ActivateModuleTutorialStepValidator : ECSBehaviour, ITutorialShowStepValidator
    {
        public bool ShowAllowed(long stepId)
        {
            GetChangeTurretTutorialValidationDataEvent eventInstance = new GetChangeTurretTutorialValidationDataEvent(stepId, 0L);
            base.ScheduleEvent(eventInstance, EngineService.EntityStub);
            return (eventInstance.BattlesCount > 1L);
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        public class UserStatisticsNode : Node
        {
            public SelfUserComponent selfUser;
            public UserStatisticsComponent userStatistics;
            public FavoriteEquipmentStatisticsComponent favoriteEquipmentStatistics;
            public KillsEquipmentStatisticsComponent killsEquipmentStatistics;
        }
    }
}

