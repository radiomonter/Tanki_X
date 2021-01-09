namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;
    using UnityEngine;

    public class BackHitDamageTutorialValidator : MonoBehaviour, ITutorialShowStepValidator
    {
        public bool ShowAllowed(long stepId)
        {
            CheckUserForSpectatorEvent eventInstance = new CheckUserForSpectatorEvent();
            EngineService.Engine.ScheduleEvent(eventInstance, EngineService.EntityStub);
            return (!eventInstance.UserIsSpectator && base.GetComponent<BackhitDamageTutorialTrigger>().canShow);
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

