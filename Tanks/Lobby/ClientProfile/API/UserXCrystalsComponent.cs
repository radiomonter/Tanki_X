namespace Tanks.Lobby.ClientProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x156fa1927a9L)]
    public class UserXCrystalsComponent : Component, ComponentServerChangeListener
    {
        private long money;

        public void ChangedOnServer(Entity entity)
        {
            EngineService.Engine.ScheduleEvent<UserXCrystalsChangedEvent>(entity);
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public long Money
        {
            get => 
                this.money;
            set => 
                this.money = value;
        }
    }
}

