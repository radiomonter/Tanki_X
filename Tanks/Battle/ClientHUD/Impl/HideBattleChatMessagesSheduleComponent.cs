namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class HideBattleChatMessagesSheduleComponent : Component
    {
        public HideBattleChatMessagesSheduleComponent()
        {
        }

        public HideBattleChatMessagesSheduleComponent(Platform.Kernel.ECS.ClientEntitySystem.API.ScheduledEvent scheduledEvent)
        {
            this.ScheduledEvent = scheduledEvent;
        }

        public Platform.Kernel.ECS.ClientEntitySystem.API.ScheduledEvent ScheduledEvent { get; set; }
    }
}

