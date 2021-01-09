namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x7f489c2bcd24568cL)]
    public class UserMoneyComponent : Component, ComponentServerChangeListener
    {
        private long money;

        public void ChangedOnServer(Entity entity)
        {
            EngineService.Engine.ScheduleEvent<UserMoneyChangedEvent>(entity);
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

