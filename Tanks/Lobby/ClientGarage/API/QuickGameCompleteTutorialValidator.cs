namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.Impl;

    public class QuickGameCompleteTutorialValidator : ECSBehaviour, ITutorialShowStepValidator
    {
        public bool ShowAllowed(long stepId)
        {
            CheckForQuickGameEvent eventInstance = new CheckForQuickGameEvent();
            base.ScheduleEvent(eventInstance, EngineService.EntityStub);
            return eventInstance.IsQuickGame;
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

