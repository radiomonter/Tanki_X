namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class ConfigEntityLoaderImpl : ConfigEntityLoader
    {
        public static readonly string STARTUP_CONFIG_PATH = "service/configentityloader";
        private static readonly string STARTUP_ROOT_NODE = "autoCreatedEntities";
        [CompilerGenerated]
        private static Func<YamlNode, ConfigEntityInfo> <>f__am$cache0;

        private int GetConfigEntityId(string path) => 
            ConfigurationEntityIdCalculator.Calculate(path);

        public void LoadEntities(Engine engine)
        {
            if (ConfigurationService.HasConfig(STARTUP_CONFIG_PATH))
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = node => node.ConvertTo<ConfigEntityInfo>();
                }
                foreach (ConfigEntityInfo info in ConfigurationService.GetConfig(STARTUP_CONFIG_PATH).GetChildListNodes(STARTUP_ROOT_NODE).Select<YamlNode, ConfigEntityInfo>(<>f__am$cache0))
                {
                    if (ConfigurationService.HasConfig(info.Path))
                    {
                        EntityInternal internal2 = SharedEntityRegistry.CreateEntity(info.TemplateId, info.Path, (long) this.GetConfigEntityId(info.Path));
                        SharedEntityRegistry.SetShared(internal2.Id);
                    }
                }
            }
        }

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.Impl.SharedEntityRegistry SharedEntityRegistry { get; set; }
    }
}

