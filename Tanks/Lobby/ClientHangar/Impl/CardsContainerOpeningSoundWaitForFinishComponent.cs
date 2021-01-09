namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CardsContainerOpeningSoundWaitForFinishComponent : Component
    {
        public CardsContainerOpeningSoundWaitForFinishComponent(ScheduleManager scheduledEvent)
        {
            this.ScheduledEvent = scheduledEvent;
        }

        public ScheduleManager ScheduledEvent { get; set; }
    }
}

