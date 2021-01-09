namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class ScheduledEventImpl : ScheduledEvent
    {
        private readonly ScheduleManager scheduleManager;

        public ScheduledEventImpl(Event scheduledEvent, ScheduleManager scheduleManager)
        {
            this.scheduleManager = scheduleManager;
        }

        public ScheduleManager Manager() => 
            this.scheduleManager;
    }
}

