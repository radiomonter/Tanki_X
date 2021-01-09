namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class FirstDamageTutorialValidator : MonoBehaviour, ITutorialShowStepValidator
    {
        [SerializeField]
        private long itemId;
        [SerializeField]
        private Slot mountSlot;

        public bool ShowAllowed(long stepId)
        {
            CheckMountedModuleEvent eventInstance = new CheckMountedModuleEvent {
                StepId = stepId,
                ItemId = this.itemId,
                MountSlot = this.mountSlot
            };
            EngineService.Engine.ScheduleEvent(eventInstance, EngineService.EntityStub);
            return eventInstance.ModuleMounted;
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

