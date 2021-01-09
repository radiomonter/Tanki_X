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
    using SharpCompress.Compressor;
    using SharpCompress.Compressor.Deflate;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    public class ConfigurationActivator : UnityAwareActivator<ManuallyCompleting>
    {
        private WWWLoader wwwLoader;

        protected override void Activate()
        {
            base.StartCoroutine(this.Load());
        }

        private string AddProfileToUrl(string url)
        {
            List<string> list = new List<string>();
            foreach (ConfigurationProfileElement element in base.GetComponents<ConfigurationProfileElement>())
            {
                list.Add(element.ProfileElement);
            }
            list.Sort();
            string currentClientVersion = StartupConfiguration.Config.CurrentClientVersion;
            string str2 = currentClientVersion.Contains("-compatible") ? currentClientVersion.Substring(0, currentClientVersion.IndexOf("-compatible", StringComparison.Ordinal)) : currentClientVersion;
            url = url + "/" + str2 + "/";
            foreach (string str3 in list)
            {
                url = url + str3 + "/";
            }
            url = url + "config.tar.gz";
            return url;
        }

        private void DisposeWWWLoader()
        {
            this.wwwLoader.Dispose();
            this.wwwLoader = null;
        }

        private void HandleError<T>() where T: Event, new()
        {
            this.DisposeWWWLoader();
            Entity entity = EngineService.Engine.CreateEntity("RemoteConfigLoading");
            EngineService.Engine.ScheduleEvent<T>(entity);
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

        [DebuggerHidden]
        private IEnumerator Load() => 
            new <Load>c__Iterator0 { $this = this };

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.EngineService EngineService { get; set; }

        [CompilerGenerated]
        private sealed class <Load>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal string <configsUrl>__0;
            internal string <url>__0;
            internal string <urlWithRandom>__0;
            internal ConfigurationActivator $this;
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
                        this.<configsUrl>__0 = InitConfiguration.Config.ConfigsUrl;
                        this.<url>__0 = this.$this.AddProfileToUrl(this.<configsUrl>__0);
                        LoggerProvider.GetLogger(this.$this).Debug("Load configs:" + this.<url>__0);
                        this.<urlWithRandom>__0 = this.<url>__0 + "?rnd=" + new Random().NextDouble();
                        this.$this.wwwLoader = new WWWLoader(new WWW(this.<urlWithRandom>__0));
                        break;

                    case 1:
                        break;

                    default:
                        goto TR_0000;
                }
                if (!this.$this.wwwLoader.IsDone)
                {
                    this.$current = null;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                if (string.IsNullOrEmpty(this.$this.wwwLoader.Error))
                {
                    if ((this.$this.wwwLoader.Bytes == null) || (this.$this.wwwLoader.Bytes.Length == 0))
                    {
                        this.$this.HandleError<GameDataLoadErrorEvent>("Empty configuration data. URL: " + this.$this.wwwLoader.URL);
                        goto TR_0001;
                    }
                    else
                    {
                        try
                        {
                            ConfigTreeNodeImpl impl;
                            using (GZipStream stream = new GZipStream(new MemoryStream(this.$this.wwwLoader.Bytes), CompressionMode.Decompress))
                            {
                                impl = new TarImporter().ImportAll<ConfigTreeNodeImpl>(stream);
                            }
                            ConfigTreeNodeImpl rootConfigNode = LocalConfiguration.rootConfigNode;
                            rootConfigNode.Add(impl);
                            ((ConfigurationServiceImpl) ConfigurationActivator.ConfigurationService).SetRootConfigNode(rootConfigNode);
                            this.$this.DisposeWWWLoader();
                            this.$this.Complete();
                            goto TR_0001;
                        }
                        catch (Exception exception)
                        {
                            this.$this.HandleError<GameDataLoadErrorEvent>($"Invalid configuration data. URL: {this.$this.wwwLoader.URL}, Error: {exception.Message}", exception);
                        }
                    }
                }
                else
                {
                    string errorMessage = $"Configuration loading was failed. URL: {this.$this.wwwLoader.URL}, Error: {this.$this.wwwLoader.Error}";
                    if ((this.$this.wwwLoader.Progress > 0f) && (this.$this.wwwLoader.Progress < 1f))
                    {
                        this.$this.HandleError<ServerDisconnectedEvent>(errorMessage);
                    }
                    else
                    {
                        this.$this.HandleError<NoServerConnectionEvent>(errorMessage);
                    }
                    goto TR_0001;
                }
            TR_0000:
                return false;
            TR_0001:
                this.$PC = -1;
                goto TR_0000;
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

