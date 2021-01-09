namespace Platform.Library.ClientUnityIntegration.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ClientUnityIntegrationActivator : DefaultActivator<AutoCompleting>, ECSActivator, Activator
    {
        protected override void Activate()
        {
            ServiceRegistry.Current.RegisterService<UnityTime>(new UnityTimeImpl());
            Protocol.RegisterCodecForType<Vector3>(new Vector3Codec());
            YamlService.RegisterConverter(new Vector3YamlConverter());
        }

        public void RegisterSystemsAndTemplates()
        {
            EngineService.RegisterSystem(new ECSToLoggerSystem());
            EngineService.RegisterSystem(new ConfigEntityLoaderSystem());
            TemplateRegistry.Register<ConfigPathCollectionTemplate>();
        }

        [Inject]
        public static Platform.Library.ClientProtocol.API.Protocol Protocol { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }

        [Inject]
        public static Platform.System.Data.Statics.ClientYaml.API.YamlService YamlService { get; set; }
    }
}

