namespace Tanks.Lobby.ClientLoading.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ClientLoadingActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        public void RegisterSystemsAndTemplates()
        {
            ECSBehaviour.EngineService.RegisterSystem(new AssetBundleLoadingProgressBarSystem());
            ECSBehaviour.EngineService.RegisterSystem(new AssetBundleLoadingSystem());
            ECSBehaviour.EngineService.RegisterSystem(new PreloadAllResourcesScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new BattleLoadScreenSystem());
            ECSBehaviour.EngineService.RegisterSystem(new OutputLogSystem());
            ECSBehaviour.EngineService.RegisterSystem(new IntroCinematicSystem());
            TemplateRegistry.Register<PreloadAllResourcesScreenTemplate>();
            TemplateRegistry.Register<LobbyLoadScreenTemplate>();
            TemplateRegistry.Register<WarmupResourcesTemplate>();
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

