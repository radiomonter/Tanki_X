namespace Platform.Library.ClientResources.Impl
{
    using Assets.platform.library.ClientResources.Scripts.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientUnityIntegration;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Runtime.CompilerServices;

    public class DiskCachingActivator : UnityAwareActivator<AutoCompleting>
    {
        protected override void Activate()
        {
            YamlNode config = ConfigurationService.GetConfig(ConfigPath.CLIENT_RESOURCES);
            string stringValue = config.GetStringValue("maximumAvailableDiskSpace");
            string str3 = config.GetStringValue("expirationDelay");
            string str4 = config.GetStringValue("compressionEnabled");
            DiskCaching.Enabled = Convert.ToBoolean(config.GetStringValue("caching"));
            DiskCaching.MaximumAvailableDiskSpace = Convert.ToInt64(stringValue);
            DiskCaching.ExpirationDelay = Convert.ToInt32(str3);
            DiskCaching.CompressionEnambled = Convert.ToBoolean(str4);
        }

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }
    }
}

