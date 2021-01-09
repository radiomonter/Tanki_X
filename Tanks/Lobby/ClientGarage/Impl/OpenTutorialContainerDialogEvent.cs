namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class OpenTutorialContainerDialogEvent : Event
    {
        public long StepId { get; set; }

        public long ItemId { get; set; }

        public int ItemsCount { get; set; }

        public TutorialContainerDialog dialog { get; set; }
    }
}

