namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using Tanks.ClientLauncher.API;
    using UnityEngine;

    public class InitConfigurationActivator : UnityAwareActivator<ManuallyCompleting>
    {
        private WWWLoader wwwLoader;

        protected override void Activate()
        {
            if (LauncherPassed)
            {
                base.Complete();
            }
            else
            {
                this.wwwLoader = new WWWLoader(new WWW(this.getInitUrl()));
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

        private string getInitUrl()
        {
            string str;
            string str2 = !new CommandLineParser(Environment.GetCommandLineArgs()).TryGetValue(LauncherConstants.TEST_SERVER, out str) ? StartupConfiguration.Config.InitUrl : ("http://" + str + ".test.tankix.com/config/init.yml");
            return (str2 + "?rnd=" + new Random().NextDouble());
        }

        private void HandleError<T>() where T: Event, new()
        {
            this.DisposeWWWLoader();
            Engine engine = EngineService.Engine;
            engine.ScheduleEvent<T>(engine.CreateEntity("InitConfigLoading"));
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
                    if (WWWLoader.GetResponseCode(this.wwwLoader.WWW) >= 400)
                    {
                        this.HandleError<TechnicalWorkEvent>();
                    }
                    else
                    {
                        this.HandleError<NoServerConnectionEvent>($"Initial config loading was failed. URL: {this.wwwLoader.URL}, Error: {this.wwwLoader.Error}");
                    }
                }
                else if ((this.wwwLoader.Bytes == null) || (this.wwwLoader.Bytes.Length == 0))
                {
                    this.HandleError<GameDataLoadErrorEvent>("Initial config is empty. URL: " + this.wwwLoader.URL);
                }
                else
                {
                    try
                    {
                        using (MemoryStream stream = new MemoryStream(this.wwwLoader.Bytes))
                        {
                            StreamReader reader = new StreamReader(stream);
                            InitConfiguration.Config = yamlService.Load<InitConfiguration>(reader);
                        }
                    }
                    catch (Exception exception)
                    {
                        this.HandleError<GameDataLoadErrorEvent>($"Invalid initial config. URL: {this.wwwLoader.URL}, Error: {exception.Message}", exception);
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
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        public static bool LauncherPassed { get; set; }
    }
}

