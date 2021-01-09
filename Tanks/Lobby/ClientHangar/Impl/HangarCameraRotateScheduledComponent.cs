namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class HangarCameraRotateScheduledComponent : Component
    {
        public HangarCameraRotateScheduledComponent()
        {
        }

        public HangarCameraRotateScheduledComponent(Platform.Kernel.ECS.ClientEntitySystem.API.ScheduledEvent scheduledEvent)
        {
            this.ScheduledEvent = scheduledEvent;
        }

        public Platform.Kernel.ECS.ClientEntitySystem.API.ScheduledEvent ScheduledEvent { get; set; }
    }
}

