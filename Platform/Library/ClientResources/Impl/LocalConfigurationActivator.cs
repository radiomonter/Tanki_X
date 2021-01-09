namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using Platform.System.Data.Statics.ClientConfigurator.Impl;
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class LocalConfigurationActivator : UnityAwareActivator<ManuallyCompleting>
    {
        private WWWLoader wwwLoader;
        private ConfigurationProfileImpl configurationProfile;

        protected override void Activate()
        {
            this.LoadConfigs();
        }

        private string[] GetProfiles()
        {
            ConfigurationProfileElement[] components = base.GetComponents<ConfigurationProfileElement>();
            if (components.Count<ConfigurationProfileElement>() == 0)
            {
                return null;
            }
            string[] strArray = new string[components.Count<ConfigurationProfileElement>()];
            for (int i = 0; i < components.Count<ConfigurationProfileElement>(); i++)
            {
                strArray[i] = components[i].ProfileElement;
            }
            return strArray;
        }

        private void HandleError()
        {
            Engine engine = EngineService.Engine;
            engine.ScheduleEvent<InvalidLocalConfigurationErrorEvent>(engine.CreateEntity("LocalConfigurationLoadingError"));
        }

        private void HandleError(string errorMessage)
        {
            LoggerProvider.GetLogger(this).Error(errorMessage);
            this.HandleError();
        }

        private void HandleError(string errorMessage, Exception e)
        {
            LoggerProvider.GetLogger(this).Error(errorMessage, e);
            this.HandleError();
        }

        private void LoadConfigs()
        {
            string path = Application.dataPath + "/" + ConfigPath.CONFIG;
            if (!Directory.Exists(path))
            {
                this.HandleError($"Local configuration folder '{path}' was not found");
            }
            else
            {
                try
                {
                    FileSystemConfigsImporter importer = new FileSystemConfigsImporter();
                    this.configurationProfile = new ConfigurationProfileImpl(null);
                    ConfigTreeNodeImpl configTreeNode = importer.Import<ConfigTreeNodeImpl>(path, this.configurationProfile);
                    ((ConfigurationServiceImpl) ConfigurationService).SetRootConfigNode(configTreeNode);
                    this.configurationProfile = new ConfigurationProfileImpl(this.GetProfiles());
                    configTreeNode = importer.Import<ConfigTreeNodeImpl>(path, this.configurationProfile);
                    ((ConfigurationServiceImpl) ConfigurationService).SetRootConfigNode(configTreeNode);
                    LocalConfiguration.rootConfigNode = configTreeNode;
                    this.SetLoadingStopTimeout();
                    base.Complete();
                }
                catch (Exception exception)
                {
                    this.HandleError($"Invalid local configuration data. Path: {path}, Error: {exception.Message}", exception);
                }
            }
        }

        private void SetLoadingStopTimeout()
        {
            try
            {
                WWWLoader.DEFAULT_TIMEOUT_SECONDS = int.Parse(ConfigurationService.GetConfig(ConfigPath.LOADING_STOP_TIMEOUT).GetStringValue("timeoutInSec"));
            }
            catch (Exception exception)
            {
                LoggerProvider.GetLogger(this).Error(exception.Message, exception);
            }
        }

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }
    }
}

