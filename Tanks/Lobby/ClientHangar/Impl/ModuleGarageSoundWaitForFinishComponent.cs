namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ModuleGarageSoundWaitForFinishComponent : Component
    {
        public ModuleGarageSoundWaitForFinishComponent(ScheduleManager scheduledEvent)
        {
            this.ScheduledEvent = scheduledEvent;
        }

        public ScheduleManager ScheduledEvent { get; set; }
    }
}

