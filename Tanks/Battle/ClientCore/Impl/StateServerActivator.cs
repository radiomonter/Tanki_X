namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientResources.Impl;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using UnityEngine;

    public class StateServerActivator : UnityAwareActivator<ManuallyCompleting>
    {
        private WWWLoader wwwLoader;
        private int state;

        protected override void Activate()
        {
            if (Environment.GetCommandLineArgs().Contains<string>("-ignorestate"))
            {
                Debug.Log("Ignoring state.yml");
                base.Complete();
            }
            else if (InitConfigurationActivator.LauncherPassed)
            {
                base.Complete();
            }
            else
            {
                string url = new CommandLineParser(Environment.GetCommandLineArgs()).GetValueOrDefault("-stateUrl", StartupConfiguration.Config.StateUrl) + "?rnd=" + new Random().NextDouble();
                this.wwwLoader = new WWWLoader(new WWW(url));
                this.wwwLoader.MaxRestartAttempts = 0;
            }
        }

        private void DisposeWWWLoader()
        {
            if (this.wwwLoader != null)
            {
                this.wwwLoader.Dispose();
                this.wwwLoader = null;
            }
        }

        private void HandleError<T>() where T: Event, new()
        {
            this.DisposeWWWLoader();
            Engine engine = ECSBehaviour.EngineService.Engine;
            engine.ScheduleEvent<T>(engine.CreateEntity("StateServerActivator"));
        }

        private void HandleError<T>(string errorMessage) where T: Event, new()
        {
            LoggerProvider.GetLogger(this).Error(errorMessage);
            this.HandleError<T>();
        }

        private void Update()
        {
            if ((this.wwwLoader != null) && this.wwwLoader.IsDone)
            {
                if (!string.IsNullOrEmpty(this.wwwLoader.Error))
                {
                    if (WWWLoader.GetResponseCode(this.wwwLoader.WWW) >= 400)
                    {
                        this.HandleError<TechnicalWorkEvent>();
                    }
                    else
                    {
                        this.HandleError<NoServerConnectionEvent>($"Configuration loading was failed. URL: {this.wwwLoader.URL}, Error: {this.wwwLoader.Error}");
                    }
                }
                else if ((this.wwwLoader.Bytes == null) || (this.wwwLoader.Bytes.Length == 0))
                {
                    this.HandleError<GameDataLoadErrorEvent>("Empty server state data. URL: " + this.wwwLoader.URL);
                }
                else
                {
                    string data = string.Empty;
                    try
                    {
                        data = Encoding.UTF8.GetString(this.wwwLoader.Bytes);
                        StateConfiguration configuration = yamlService.Load<StateConfiguration>(data);
                        this.state = configuration.State;
                        if (this.state != 0)
                        {
                            this.HandleError<TechnicalWorkEvent>();
                        }
                    }
                    catch (Exception exception)
                    {
                        this.HandleError<GameDataLoadErrorEvent>($"Invalid configuration data. URL: {this.wwwLoader.URL}, Error: {exception.Message}, Data: {data}");
                        return;
                    }
                    this.DisposeWWWLoader();
                    base.Complete();
                }
            }
        }

        [Inject]
        public static YamlService yamlService { get; set; }
    }
}

