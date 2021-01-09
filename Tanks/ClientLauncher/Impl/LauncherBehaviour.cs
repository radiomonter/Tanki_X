namespace Tanks.ClientLauncher.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientResources.Impl;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using Tanks.ClientLauncher.API;
    using Tanks.Lobby.ClientNavigation.API;
    using Tanks.Lobby.ClientNavigation.Impl;
    using UnityEngine;

    public class LauncherBehaviour : MonoBehaviour
    {
        [SerializeField]
        private GameObject downloadScreen;
        [SerializeField]
        private GameObject errorDialogScreen;
        private static LauncherBehaviour instance;
        private string currentVersion;
        private string remoteVersion;
        private string distributionUrl;
        private string executable;

        private void Awake()
        {
            instance = this;
        }

        private bool CheckUpdateReport()
        {
            bool isSuccess;
            if (!new CommandLineParser(Environment.GetCommandLineArgs()).IsExist(LauncherConstants.UPDATE_REPORT_COMMAND))
            {
                return true;
            }
            UpdateReport report = new UpdateReport();
            string str = string.Empty;
            string updateVersion = string.Empty;
            try
            {
                using (FileStream stream = new FileStream(ApplicationUtils.GetAppRootPath() + "/" + LauncherConstants.REPORT_FILE_NAME, FileMode.Open))
                {
                    report.Read(stream);
                    isSuccess = report.IsSuccess;
                    str = report.Error + report.StackTrace;
                    updateVersion = report.UpdateVersion;
                }
            }
            catch (Exception exception1)
            {
                isSuccess = false;
                str = exception1.ToString();
            }
            if (!isSuccess)
            {
                LoggerProvider.GetLogger(this).ErrorFormat("ClientUpdateError: {0}", str);
                EngineService.Engine.NewEvent<ClientUpdateErrorEvent>().Schedule();
                return false;
            }
            if (!this.IsCurrentVersionNeedsUpdate())
            {
                return true;
            }
            LoggerProvider.GetLogger(this).ErrorFormat("ClientUpdateError: Updated version is not correct, update version = {0}, currentVersion = {1}", report.UpdateVersion, this.currentVersion);
            EngineService.Engine.NewEvent<ClientUpdateErrorEvent>().Schedule();
            return false;
        }

        private bool IsCurrentVersionNeedsUpdate()
        {
            if (this.remoteVersion == this.currentVersion)
            {
                return false;
            }
            if (string.IsNullOrEmpty(this.currentVersion) || string.IsNullOrEmpty(this.remoteVersion))
            {
                return true;
            }
            string str = this.currentVersion.Contains("-compatible") ? this.currentVersion.Substring(0, this.currentVersion.IndexOf("-compatible", StringComparison.Ordinal)) : this.currentVersion;
            return (str != (this.remoteVersion.Contains("-compatible") ? this.remoteVersion.Substring(0, this.remoteVersion.IndexOf("-compatible", StringComparison.Ordinal)) : this.remoteVersion));
        }

        public void Launch()
        {
            this.ReadConfigs();
            if (this.CheckUpdateReport())
            {
                this.UpdateClientOrStartGame();
            }
        }

        private void ReadConfigs()
        {
            this.currentVersion = ConfigurationService.GetConfig(ConfigPath.STARTUP).GetStringValue("currentClientVersion");
            this.remoteVersion = UpdateConfiguration.Config.LastClientVersion;
            this.distributionUrl = UpdateConfiguration.Config.DistributionUrl;
            this.executable = UpdateConfiguration.Config.Executable;
        }

        public static void RetryUpdate()
        {
            instance.UpdateClientOrStartGame();
        }

        private void StartClientDownload()
        {
            this.downloadScreen.SetActive(true);
            this.downloadScreen.GetComponent<ClientDownloadBehaviour>().Init(this.remoteVersion, this.distributionUrl, this.executable);
        }

        private void StartGame()
        {
            Entity entity = EngineService.Engine.CreateEntity("StartGame");
            SceneSwitcher.CleanAndSwitch(SceneNames.ENTRANCE);
            InitConfigurationActivator.LauncherPassed = true;
        }

        private void UpdateClientOrStartGame()
        {
            if (new CommandLineParser(Environment.GetCommandLineArgs()).IsExist(LauncherConstants.NO_UPDATE_COMMAND))
            {
                this.StartGame();
            }
            else if (this.IsCurrentVersionNeedsUpdate())
            {
                this.StartClientDownload();
            }
            else
            {
                this.StartGame();
            }
        }

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }
    }
}

