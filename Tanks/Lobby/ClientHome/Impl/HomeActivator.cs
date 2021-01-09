namespace Tanks.Lobby.ClientHome.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    public class HomeActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        public void RegisterSystemsAndTemplates()
        {
            TemplateRegistry.Register<HomeScreenTemplate>();
            ECSBehaviour.EngineService.RegisterSystem(new HomeScreenSystem());
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }
    }
}

