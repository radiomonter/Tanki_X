namespace Platform.Library.ClientResources.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Runtime.CompilerServices;

    public class AssetReferenceComponent : Component
    {
        public AssetReferenceComponent()
        {
        }

        public AssetReferenceComponent(AssetReference reference)
        {
            this.Reference = reference;
        }

        public static AssetReferenceComponent createFromConfig(string configPath)
        {
            YamlNode config = ConfigurationService.GetConfig(configPath);
            if (config == null)
            {
                throw new ArgumentException("Not found config '" + configPath + "'");
            }
            YamlNode childNode = config.GetChildNode("unityAsset");
            if (childNode == null)
            {
                throw new ArgumentException("Not found unityAsset in config '" + configPath + "'");
            }
            return childNode.ConvertTo<AssetReferenceComponent>();
        }

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }

        public AssetReference Reference { get; set; }
    }
}

