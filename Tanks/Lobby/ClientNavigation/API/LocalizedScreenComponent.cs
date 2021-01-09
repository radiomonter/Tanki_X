namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    public abstract class LocalizedScreenComponent : FromConfigBehaviour, Component, AttachToEntityListener
    {
        protected LocalizedScreenComponent()
        {
        }

        public void AttachedToEntity(Entity entity)
        {
            SetScreenHeaderEvent eventInstance = new SetScreenHeaderEvent();
            eventInstance.Animated(this.Header);
            EngineService.Engine.ScheduleEvent(eventInstance, entity);
        }

        public void DetachFromEntity(Entity entity)
        {
        }

        protected override string GetRelativeConfigPath() => 
            "/ui/screen";

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        public string Header { private get; set; }
    }
}

