namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class ChangeTurretTutorialStepValidator : ECSBehaviour, ITutorialShowStepValidator
    {
        [SerializeField]
        private long itemId;

        public bool ShowAllowed(long stepId)
        {
            GetChangeTurretTutorialValidationDataEvent eventInstance = new GetChangeTurretTutorialValidationDataEvent(stepId, this.itemId);
            base.ScheduleEvent(eventInstance, EngineService.EntityStub);
            return ((eventInstance.BattlesCount > 0L) && !eventInstance.TutorialItemAlreadyMounted);
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

