namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Runtime.CompilerServices;

    public class StartupConfigurationActivator : UnityAwareActivator<AutoCompleting>
    {
        protected override void Activate()
        {
            try
            {
                StartupConfiguration.Config = ConfigurationService.GetConfig(ConfigPath.STARTUP).ConvertTo<StartupConfiguration>();
            }
            catch (Exception exception)
            {
                this.HandleError<InvalidLocalConfigurationErrorEvent>($"Invalid local configuration. Error: {exception.Message}", exception);
            }
        }

        private void HandleError<T>(string errorMessage, Exception e) where T: Event, new()
        {
            LoggerProvider.GetLogger(this).Error(errorMessage, e);
            Engine engine = EngineService.Engine;
            engine.ScheduleEvent<T>(engine.CreateEntity("StartupConfigLoading"));
        }

        [Inject]
        public static Platform.System.Data.Statics.ClientYaml.API.YamlService YamlService { get; set; }

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }
    }
}

