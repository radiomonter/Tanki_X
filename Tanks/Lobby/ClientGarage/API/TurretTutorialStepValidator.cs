namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class TurretTutorialStepValidator : ECSBehaviour, ITutorialShowStepValidator
    {
        [SerializeField]
        private long neededWeapon;

        public bool ShowAllowed(long stepId)
        {
            GetChangeTurretTutorialValidationDataEvent eventInstance = new GetChangeTurretTutorialValidationDataEvent(stepId, 0L);
            base.ScheduleEvent(eventInstance, EngineService.EntityStub);
            return (eventInstance.MountedWeaponId == this.neededWeapon);
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

