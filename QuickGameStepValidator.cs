using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.OSGi.ClientCore.API;
using Platform.Library.ClientUnityIntegration.API;
using System;
using System.Runtime.CompilerServices;
using Tanks.Lobby.ClientGarage.API;
using Tanks.Lobby.ClientGarage.Impl;

public class QuickGameStepValidator : ECSBehaviour, ITutorialShowStepValidator
{
    public bool ShowAllowed(long stepId)
    {
        GetEnergyCountTutorialValidationDataEvent eventInstance = new GetEnergyCountTutorialValidationDataEvent();
        base.ScheduleEvent(eventInstance, EngineService.EntityStub);
        return (eventInstance.Quantums <= 0L);
    }

    [Inject]
    public static EngineServiceInternal EngineService { get; set; }
}

