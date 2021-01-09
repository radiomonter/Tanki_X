namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class OpenTutorialContainerEvent : Event
    {
        public long StepId { get; set; }

        public long ItemId { get; set; }
    }
}

