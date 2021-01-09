namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientHangar.API;

    public class ClientHangarActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        protected override void Activate()
        {
            base.CreateEntity<HangarTemplate>("/hangar");
        }

        public void RegisterSystemsAndTemplates()
        {
            ECSBehaviour.EngineService.RegisterSystem(new AssetsFirstLoadingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HangarBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HangarCameraSwitchSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HangarTankBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HangarTankLoadSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ItemPreviewBaseSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ItemPreviewSystem());
            ECSBehaviour.EngineService.RegisterSystem(new ContainerItemPreviewSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HangarGraffitiBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HangarContainerBuilderSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HangarCameraControlSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HangarCameraRotateSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HangarCameraAutoRotateSystem());
            ECSBehaviour.EngineService.RegisterSystem(new UserReadyForLobbySystem());
            ECSBehaviour.EngineService.RegisterSystem(new HangarAmbientSoundSystem());
            ECSBehaviour.EngineService.RegisterSystem(new CardsContainerSoundSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HangarModuleSoundsSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HangarCameraFlightToLocationSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HangarCameraFlightToTankSystem());
            ECSBehaviour.EngineService.RegisterSystem(new HangarCameraFlightSystem());
            TemplateRegistry.Register(typeof(HangarTemplate));
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }

        [Inject]
        public static Tanks.Battle.ClientCore.Impl.InputManager InputManager { get; set; }
    }
}

