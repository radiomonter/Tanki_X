namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Runtime.CompilerServices;

    public class TemplateAccessor
    {
        private readonly Platform.Kernel.ECS.ClientEntitySystem.API.TemplateDescription templateDescription;
        public string configPath;
        private readonly Platform.System.Data.Statics.ClientYaml.API.YamlNode yamlNode;

        private TemplateAccessor(Platform.Kernel.ECS.ClientEntitySystem.API.TemplateDescription templateDescription)
        {
            this.templateDescription = templateDescription;
        }

        public TemplateAccessor(Platform.Kernel.ECS.ClientEntitySystem.API.TemplateDescription templateDescription, Platform.System.Data.Statics.ClientYaml.API.YamlNode yamlNode) : this(templateDescription)
        {
            this.yamlNode = yamlNode;
        }

        public TemplateAccessor(Platform.Kernel.ECS.ClientEntitySystem.API.TemplateDescription templateDescription, string configPath) : this(templateDescription)
        {
            this.configPath = configPath;
        }

        public bool HasConfigPath() => 
            !string.IsNullOrEmpty(this.configPath);

        [Inject]
        public static ConfigurationService ConfiguratorService { get; set; }

        public virtual Platform.Kernel.ECS.ClientEntitySystem.API.TemplateDescription TemplateDescription =>
            this.templateDescription;

        public Platform.System.Data.Statics.ClientYaml.API.YamlNode YamlNode =>
            !this.HasConfigPath() ? this.yamlNode : ConfiguratorService.GetConfig(this.ConfigPath);

        public string ConfigPath
        {
            get
            {
                if (!this.HasConfigPath())
                {
                    throw new CannotAccessPathForTemplate(this.TemplateDescription.TemplateClass);
                }
                return this.configPath;
            }
            set => 
                this.configPath = value;
        }
    }
}

