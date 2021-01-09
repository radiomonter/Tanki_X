namespace Tanks.Lobby.ClientControls.API
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class LocalizationUtils
    {
        public static readonly string CONFIG_PATH = "localization";

        public static string Localize(string uid)
        {
            YamlNode config = ConfigurationService.GetConfig(CONFIG_PATH);
            try
            {
                using (Dictionary<object, object>.Enumerator enumerator = ((Dictionary<object, object>) config.GetValue(uid)).GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        return (string) enumerator.Current.Value;
                    }
                }
                return string.Empty;
            }
            catch (KeyNotFoundException)
            {
                return string.Empty;
            }
        }

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }
    }
}

