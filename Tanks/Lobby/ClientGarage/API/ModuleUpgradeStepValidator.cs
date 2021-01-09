﻿namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class ModuleUpgradeStepValidator : ECSBehaviour, ITutorialShowStepValidator
    {
        [SerializeField]
        private long moduleId;

        public bool ShowAllowed(long stepId)
        {
            CheckModuleForUpgradeEvent eventInstance = new CheckModuleForUpgradeEvent {
                StepId = stepId,
                ItemId = this.moduleId
            };
            base.ScheduleEvent(eventInstance, EngineService.EntityStub);
            return eventInstance.UpgradeAvailable;
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

