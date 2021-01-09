namespace Platform.Library.ClientUnityIntegration.API
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using Platform.System.Data.Statics.ClientYaml.Impl;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;
    using UnityEngine;
    using YamlDotNet.Serialization;

    public class FromConfigBehaviour : ECSBehaviour
    {
        [HideInInspector, SerializeField]
        private string configPath;
        [HideInInspector, SerializeField]
        private string yamlKey;
        private static Regex namespaceToConfigPath = new Regex(@"(\.Impl.*)|(\.API.*)|(Client)", RegexOptions.IgnoreCase);
        private static Regex specialNames = new Regex("(Component)|(Text)|(Texts)|(Localization)|(LocalizedStrings)", RegexOptions.IgnoreCase);
        private static readonly INamingConvention naming = new PascalToCamelCaseNamingConvertion();

        protected virtual void Awake()
        {
            this.GetValuesFromConfig();
        }

        protected virtual string GetRelativeConfigPath() => 
            string.Empty;

        private void GetValuesFromConfig()
        {
            YamlNode childNode = ConfigurationService.GetConfig(this.ConfigPath).GetChildNode(this.YamlKey);
            UnityUtil.SetPropertiesFromYamlNode(this, childNode, new PascalToCamelCaseNamingConvertion());
        }

        private string NamespaceToConfigPath() => 
            namespaceToConfigPath.Replace(base.GetType().Namespace, string.Empty).Replace(".", "/").ToLower() + this.GetRelativeConfigPath();

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }

        public virtual string ConfigPath =>
            !this.IsStaticConfigPath ? this.configPath : this.NamespaceToConfigPath();

        public virtual string YamlKey =>
            !this.IsStaticYamlKey ? this.yamlKey : naming.Apply(specialNames.Replace(base.GetType().Name, string.Empty));

        public virtual bool IsStaticYamlKey =>
            true;

        public virtual bool IsStaticConfigPath =>
            true;
    }
}

