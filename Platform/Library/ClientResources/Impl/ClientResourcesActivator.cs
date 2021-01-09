namespace Platform.Library.ClientResources.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientLogger.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.Library.ClientUnityIntegration.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using SharpCompress.Compressor;
    using SharpCompress.Compressor.Deflate;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using UnityEngine;

    public class ClientResourcesActivator : UnityAwareActivator<ManuallyCompleting>, ECSActivator, Activator
    {
        private WWWLoader dbLoader;
        private Entity dbEntity;

        protected override void Activate()
        {
            this.dbEntity = EngineService.Engine.CreateEntity("AssetBundleDatabase");
            bool flag = true;
            string baseUrl = (InitConfiguration.Config.ResourcesUrl + "/" + BuildTargetName.GetName()).Replace("{DataPath}", Application.dataPath);
            string str2 = !"LATEST".Equals(InitConfiguration.Config.BundleDbVersion) ? ("-" + InitConfiguration.Config.BundleDbVersion) : string.Empty;
            string url = !flag ? AssetBundleNaming.GetAssetBundleUrl(baseUrl, AssetBundleNaming.DB_PATH + str2) : (((Application.platform != RuntimePlatform.WindowsPlayer) ? "file://" : "file:///") + Application.dataPath + "/" + AssetBundleNaming.DB_FILENAME);
            WWWLoader loader = new WWWLoader(new WWW(url)) {
                MaxRestartAttempts = 0
            };
            this.dbLoader = loader;
            BaseUrlComponent component = new BaseUrlComponent {
                Url = baseUrl + "/"
            };
            this.dbEntity.AddComponent(component);
        }

        public AssetBundleDatabase DeserializeDatabase(byte[] bytes)
        {
            string str = string.Empty;
            if (true)
            {
                str = Encoding.UTF8.GetString(bytes);
                if (string.IsNullOrEmpty(str))
                {
                    throw new Exception("AssetBundleDatabase data is empty");
                }
            }
            else
            {
                using (GZipStream stream = new GZipStream(new MemoryStream(bytes), CompressionMode.Decompress))
                {
                    int count = 0xa00000;
                    byte[] buffer = new byte[count];
                    int num2 = stream.Read(buffer, 0, count);
                    if ((num2 < bytes.Length) || (num2 == count))
                    {
                        throw new Exception("Decompress failed. read=" + num2);
                    }
                    str = new UTF8Encoding().GetString(buffer, 0, num2);
                }
            }
            AssetBundleDatabase database = new AssetBundleDatabase();
            database.Deserialize(str);
            return database;
        }

        private void RegisterSystems()
        {
            EngineService.RegisterSystem(new AssetStorageSystem());
            EngineService.RegisterSystem(new AssetAsyncLoaderSystem());
            EngineService.RegisterSystem(new AssetBundleLoadSystem());
            EngineService.RegisterSystem(new AssetBundleStorageSystem());
            EngineService.RegisterSystem(new AssetLoadStatSystem());
            EngineService.RegisterSystem(new UrlLoadSystem());
            EngineService.RegisterSystem(new AssetBundlePreloadSystem());
            EngineService.RegisterSystem(new AssetLoadByEventSystem());
            EngineService.RegisterSystem(new AssetBundleDiskCacheSystem());
        }

        public void RegisterSystemsAndTemplates()
        {
            this.RegisterSystems();
        }

        private void Update()
        {
            if ((this.dbLoader != null) && this.dbLoader.IsDone)
            {
                Engine engine = EngineService.Engine;
                base.enabled = false;
                if (!string.IsNullOrEmpty(this.dbLoader.Error))
                {
                    string message = $"AssetBundleDatabase loading was failed. URL: {this.dbLoader.URL}, Error: {this.dbLoader.Error}";
                    LoggerProvider.GetLogger(this).Error(message);
                    this.dbLoader.Dispose();
                    this.dbLoader = null;
                    engine.ScheduleEvent<InvalidGameDataErrorEvent>(engine.CreateEntity("RemoteConfigLoading"));
                }
                else
                {
                    this.dbLoader.Dispose();
                    this.dbLoader = null;
                    AssetBundleDatabaseComponent component = new AssetBundleDatabaseComponent {
                        AssetBundleDatabase = this.DeserializeDatabase(this.dbLoader.Bytes)
                    };
                    this.dbEntity.AddComponent(component);
                    base.Complete();
                }
            }
        }

        [Inject]
        public static EngineServiceInternal EngineService { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.TemplateRegistry TemplateRegistry { get; set; }

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }
    }
}

