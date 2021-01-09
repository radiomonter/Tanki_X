namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class TutorialStepIndexEvent : Event
    {
        public int CurrentStepNumber { get; set; }

        public int StepCountInTutorial { get; set; }
    }
}

