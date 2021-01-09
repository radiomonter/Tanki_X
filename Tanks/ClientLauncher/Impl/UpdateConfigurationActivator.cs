namespace Tanks.ClientLauncher.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientResources.Impl;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class UpdateConfigurationActivator : UnityAwareActivator<ManuallyCompleting>
    {
        private WWWLoader wwwLoader;

        protected override void Activate()
        {
            if (InitConfigurationActivator.LauncherPassed)
            {
                base.Complete();
            }
            else
            {
                string url = InitConfiguration.Config.UpdateConfigUrl.Replace("{DataPath}", Application.dataPath).Replace("{BuildTarget}", BuildTargetName.GetName());
                this.wwwLoader = new WWWLoader(new WWW(url));
            }
        }

        private void DisposeWWWLoader()
        {
            this.wwwLoader.Dispose();
            this.wwwLoader = null;
        }

        private void HandleError<T>() where T: Event, new()
        {
            this.DisposeWWWLoader();
            Engine engine = ECSBehaviour.EngineService.Engine;
            engine.ScheduleEvent<T>(engine.CreateEntity("UpdateConfigurationLoading"));
        }

        private void HandleError<T>(string errorMessage) where T: Event, new()
        {
            LoggerProvider.GetLogger(this).Error(errorMessage);
            this.HandleError<T>();
        }

        private void HandleError<T>(string errorMessage, Exception e) where T: Event, new()
        {
            LoggerProvider.GetLogger(this).Error(errorMessage, e);
            this.HandleError<T>();
        }

        private void Update()
        {
            if ((this.wwwLoader != null) && this.wwwLoader.IsDone)
            {
                if (!string.IsNullOrEmpty(this.wwwLoader.Error))
                {
                    string errorMessage = $"Update configuration loading was failed. URL: {this.wwwLoader.URL}, Error: {this.wwwLoader.Error}";
                    if ((this.wwwLoader.Progress > 0f) && (this.wwwLoader.Progress < 1f))
                    {
                        this.HandleError<ServerDisconnectedEvent>(errorMessage);
                    }
                    else
                    {
                        this.HandleError<NoServerConnectionEvent>(errorMessage);
                    }
                }
                else if ((this.wwwLoader.Bytes == null) || (this.wwwLoader.Bytes.Length == 0))
                {
                    this.HandleError<GameDataLoadErrorEvent>("Empty update configuration data. URL: " + this.wwwLoader.URL);
                }
                else
                {
                    try
                    {
                        using (MemoryStream stream = new MemoryStream(this.wwwLoader.Bytes))
                        {
                            StreamReader reader = new StreamReader(stream);
                            UpdateConfiguration.Config = yamlService.Load<UpdateConfiguration>(reader);
                        }
                    }
                    catch (Exception exception)
                    {
                        this.HandleError<GameDataLoadErrorEvent>($"Invalid update configuration data. URL: {this.wwwLoader.URL}, Error: {exception.Message}", exception);
                        return;
                    }
                    this.DisposeWWWLoader();
                    base.Complete();
                }
            }
        }

        [Inject]
        public static YamlService yamlService { get; set; }

        [Inject]
        public static ConfigurationService configurationService { get; set; }
    }
}

