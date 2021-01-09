namespace Tanks.ClientLauncher.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using System;
    using System.Runtime.CompilerServices;

    public class ECSRunActivator : UnityAwareActivator<AutoCompleting>, ECSActivator, Activator
    {
        public void RegisterSystemsAndTemplates()
        {
            EngineService.RunECSKernel();
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

