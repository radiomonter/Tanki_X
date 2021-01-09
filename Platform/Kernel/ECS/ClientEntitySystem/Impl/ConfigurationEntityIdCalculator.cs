namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ConfigurationEntityIdCalculator
    {
        public static int Calculate(string path)
        {
            path = path.Replace(@"\", "/");
            if (path.StartsWith("/", StringComparison.Ordinal))
            {
                path = path.Substring(1);
            }
            if (path.EndsWith("/", StringComparison.Ordinal))
            {
                path = path.Substring(0, path.Length - 1);
            }
            if (ConfigurationService.HasConfig(path))
            {
                YamlNode config = ConfigurationService.GetConfig(path);
                if (config.HasValue("id"))
                {
                    return int.Parse(config.GetStringValue("id"));
                }
            }
            return path.GetHashCode();
        }

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }
    }
}

