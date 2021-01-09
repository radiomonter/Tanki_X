namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.Impl;

    public class TutorialStepHandler : BehaviourComponent
    {
        public TutorialData tutorialData;

        public virtual void RunStep(TutorialData data)
        {
            this.tutorialData = data;
        }

        protected virtual void StepComplete()
        {
            base.ScheduleEvent<TutorialStepCompleteEvent>(this.tutorialData.TutorialStep);
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

