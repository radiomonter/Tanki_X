namespace Tanks.Lobby.ClientProfile.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class TanksGraphicsSettingsAnalyzer : GraphicsSettingsAnalyzer
    {
        private const string SPACE = " ";
        private const string UNSUPPORTED_QUALITY_NAME = "Unsupported";
        [SerializeField]
        private string configPath;
        private Quality maxDefaultQuality = Quality.maximum;
        private Quality defaultQualityForUnknownDevice = Quality.maximum;
        private Quality minQuality = Quality.fastest;
        private bool unsupportedDevice;
        private Quality defaultQuality;
        [CompilerGenerated]
        private static Func<Resolution, int> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<KeyValuePair<string, string>, string> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<KeyValuePair<string, string>, string> <>f__am$cache2;

        private Dictionary<string, string> GetConfigData()
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = k => PrepareDeviceName(k.Key);
            }
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = k => k.Value;
            }
            return Configuration.GetConfig(this.configPath).ConvertTo<Dictionary<string, string>>().ToDictionary<KeyValuePair<string, string>, string, string>(<>f__am$cache1, <>f__am$cache2);
        }

        public override Quality GetDefaultQuality() => 
            this.defaultQuality;

        public override Resolution GetDefaultResolution(List<Resolution> resolutions)
        {
            <>f__am$cache0 ??= r => (r.width + r.height);
            Func<Resolution, int> keySelector = <>f__am$cache0;
            return (!this.unsupportedDevice ? resolutions.OrderByDescending<Resolution, int>(keySelector).First<Resolution>() : resolutions.OrderBy<Resolution, int>(keySelector).First<Resolution>());
        }

        public override void Init()
        {
            Dictionary<string, string> configData = this.GetConfigData();
            string key = PrepareDeviceName(SystemInfo.graphicsDeviceName);
            if (!configData.ContainsKey(key))
            {
                Console.WriteLine("Unknown device {0}! Default Quality Level = {1}", key, this.defaultQualityForUnknownDevice.Name);
                this.defaultQuality = this.defaultQualityForUnknownDevice;
            }
            else
            {
                string qualityName = configData[key];
                if (!qualityName.Equals("Unsupported"))
                {
                    Quality qualityByName = Quality.GetQualityByName(qualityName);
                    this.defaultQuality = (qualityByName.Level <= this.maxDefaultQuality.Level) ? qualityByName : this.maxDefaultQuality;
                }
                else
                {
                    this.unsupportedDevice = true;
                    this.defaultQuality = this.minQuality;
                    Console.WriteLine("Unsupported device! Default Quality Level = " + this.minQuality.Name);
                }
            }
        }

        private static string PrepareDeviceName(string currentDeviceName) => 
            currentDeviceName.Replace(" ", string.Empty).Trim().ToUpperInvariant();

        [Inject]
        public static ConfigurationService Configuration { get; set; }
    }
}

