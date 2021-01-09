namespace Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1535aaa4b09L)]
    public class ConfirmedUserEmailComponent : Component, ComponentServerChangeListener
    {
        private string email;

        void ComponentServerChangeListener.ChangedOnServer(Entity entity)
        {
            EngineService.Engine.ScheduleEvent<ConfirmedUserEmailChangedEvent>(entity);
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public string Email
        {
            get => 
                this.email;
            set => 
                this.email = value;
        }

        public bool Subscribed { get; set; }
    }
}

