namespace Tanks.Lobby.ClientNavigation.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    public class NavigationActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        protected override void Activate()
        {
            Entity entity = ECSBehaviour.EngineService.Engine.CreateEntity("Navigation");
            entity.AddComponent<HistoryComponent>();
            entity.AddComponent<CurrentScreenComponent>();
            entity.AddComponent<ScreensRegistryComponent>();
        }

        public void RegisterSystemsAndTemplates()
        {
            TemplateRegistry.Register<ScreenTemplate>();
            ECSBehaviour.EngineService.RegisterSystem(new TopPanelSystem());
            ECSBehaviour.EngineService.RegisterSystem(new AttachToScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new NavigationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ScreenLockSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ElementLockSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BackgroundSystem());
            ECSBehaviour.EngineService.RegisterSystem(new LoadingErrorsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new SceneLoaderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new GoBackSoundEffectSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ApplicationExitSystem());
            ECSBehaviour.EngineService.RegisterSystem(new NavigationStatisticsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new DialogsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new LinkNavigationSystem());
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

