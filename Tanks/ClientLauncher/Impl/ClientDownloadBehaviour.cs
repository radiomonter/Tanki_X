namespace Tanks.ClientLauncher.Impl
{
    using log4net;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientControls.API;
    using Tanks.ClientLauncher.API;
    using UnityEngine;

    public class ClientDownloadBehaviour : MonoBehaviour
    {
        private string version;
        private string url;
        private string executable;
        private string updatePath;
        private ProgressBarComponent progressBar;
        private WWWLoader www;

        private void CompleteDownloading()
        {
            if (!string.IsNullOrEmpty(this.www.Error))
            {
                base.enabled = false;
                this.Log($"Loading failed. URL: {this.www.URL}, Error: {this.www.Error}", null);
                this.SendErrorEvent();
            }
            else
            {
                try
                {
                    this.ExtractFiles();
                    this.Reboot();
                }
                catch (Exception exception)
                {
                    this.Log(exception.Message, exception);
                    this.SendErrorEvent();
                }
            }
        }

        private void ExtractFiles()
        {
            try
            {
                if (Directory.Exists(this.updatePath))
                {
                    FileUtils.DeleteDirectory(this.updatePath);
                }
            }
            catch (Exception)
            {
                this.updatePath = this.updatePath + "_alt";
            }
            try
            {
                if (Directory.Exists(this.updatePath))
                {
                    FileUtils.DeleteDirectory(this.updatePath);
                }
            }
            catch (Exception exception2)
            {
                this.Log(exception2.Message, exception2);
            }
            using (MemoryStream stream = new MemoryStream(this.www.Bytes))
            {
                TarUtility.Extract(stream, this.updatePath);
            }
        }

        public void Init(string version, string url, string executable)
        {
            this.updatePath = Path.GetTempPath() + "/" + LauncherConstants.UPDATE_PATH;
            this.url = url;
            this.version = version;
            this.executable = executable;
        }

        private void Log(string message, Exception e)
        {
            string str = "ClientUpdateError: " + message;
            ILog logger = LoggerProvider.GetLogger(this);
            if (e == null)
            {
                logger.Error(str);
            }
            else
            {
                logger.Error(str, e);
            }
        }

        private void Reboot()
        {
            EngineService.Engine.NewEvent<StartRebootEvent>().Attach(EngineService.EntityStub).Schedule();
            string appRootPath = ApplicationUtils.GetAppRootPath();
            string subLine = new CommandLineParser(Environment.GetCommandLineArgs()).GetSubLine(LauncherConstants.PASS_THROUGH);
            string args = $"-batchmode -nographics {LauncherConstants.UPDATE_PROCESS_COMMAND}={Process.GetCurrentProcess().Id} {LauncherConstants.VERSION_COMMAND}={this.version} {LauncherConstants.PARENT_PATH_COMMAND}={ApplicationUtils.WrapPath(appRootPath)} {subLine}";
            try
            {
                ApplicationUtils.StartProcessAsAdmin(this.updatePath + "/" + ApplicationUtils.GetExecutablePathByName(this.executable), args);
            }
            catch
            {
                ApplicationUtils.StartProcess(this.updatePath + "/" + ApplicationUtils.GetExecutablePathByName(this.executable), args);
            }
            base.StartCoroutine(this.WaitAndReboot(2f));
        }

        private void SendErrorEvent()
        {
            EngineService.Engine.NewEvent<ClientUpdateErrorEvent>().Schedule();
        }

        private void Start()
        {
            base.StartCoroutine(this.WaitAndStartDownLoad(0.5f));
        }

        private void Update()
        {
            if (this.www != null)
            {
                this.progressBar.ProgressValue = this.www.Progress;
                if (this.www.IsDone)
                {
                    this.progressBar.ProgressValue = 1f;
                    this.CompleteDownloading();
                    this.www.Dispose();
                    this.www = null;
                }
            }
        }

        [DebuggerHidden]
        private IEnumerator WaitAndReboot(float waitTime) => 
            new <WaitAndReboot>c__Iterator1 { waitTime = waitTime };

        [DebuggerHidden]
        private IEnumerator WaitAndStartDownLoad(float waitTime) => 
            new <WaitAndStartDownLoad>c__Iterator0 { 
                waitTime = waitTime,
                $this = this
            };

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        [CompilerGenerated]
        private sealed class <WaitAndReboot>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal float waitTime;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$current = new WaitForSeconds(this.waitTime);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        Application.Quit();
                        Process.GetCurrentProcess().Kill();
                        this.$PC = -1;
                        break;

                    default:
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }

        [CompilerGenerated]
        private sealed class <WaitAndStartDownLoad>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal float waitTime;
            internal ClientDownloadBehaviour $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        this.$current = new WaitForSeconds(this.waitTime);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        ClientDownloadBehaviour.EngineService.Engine.NewEvent<StartDownloadEvent>().Attach(ClientDownloadBehaviour.EngineService.EntityStub).Schedule();
                        this.$this.progressBar = this.$this.GetComponentInChildren<ProgressBarComponent>();
                        this.$this.progressBar.ProgressValue = 0f;
                        this.$this.www = new WWWLoader(new WWW(this.$this.url));
                        this.$PC = -1;
                        break;

                    default:
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

