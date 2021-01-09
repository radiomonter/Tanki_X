namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class ModuleResearchStepValidator : ECSBehaviour, ITutorialShowStepValidator
    {
        [SerializeField]
        private long moduleId;

        public bool ShowAllowed(long stepId)
        {
            CheckModuleForResearchEvent eventInstance = new CheckModuleForResearchEvent {
                StepId = stepId,
                ItemId = this.moduleId
            };
            base.ScheduleEvent(eventInstance, EngineService.EntityStub);
            return eventInstance.ResearchAvailable;
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

