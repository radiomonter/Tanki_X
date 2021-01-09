namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientDataStructures.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Runtime.InteropServices;

    public class EntityBuilder
    {
        private static long idCounter = 0x100000000L;
        protected readonly EngineServiceInternal engineServiceInternal;
        private readonly EntityRegistry entityRegistry;
        protected readonly TemplateRegistry templateRegistry;
        protected long? id;
        private string configPath;
        protected string name;
        private TemplateDescription templateDescription;
        private YamlNode templateYamlNode;
        private Optional<TemplateAccessor> templateAccessor;

        public EntityBuilder(EngineServiceInternal engineServiceInternal, EntityRegistry entityRegistry, TemplateRegistry templateRegistry)
        {
            this.engineServiceInternal = engineServiceInternal;
            this.entityRegistry = entityRegistry;
            this.templateRegistry = templateRegistry;
        }

        public EntityInternal Build(bool registerInEngine = true)
        {
            if (this.id == null)
            {
                idCounter += 1L;
                this.id = new long?(idCounter);
            }
            this.name ??= ((this.templateDescription == null) ? Convert.ToString(this.id) : this.templateDescription.TemplateName);
            if (!this.templateAccessor.IsPresent())
            {
                this.templateAccessor = this.CreateTemplateAccessor();
            }
            this.ResolveConfigPathByTemplate(this.templateAccessor);
            EntityImpl entity = this.CreateEntity(this.templateAccessor);
            if (registerInEngine)
            {
                this.entityRegistry.RegisterEntity(entity);
                entity.AddComponent(typeof(NewEntityComponent));
            }
            return entity;
        }

        protected virtual EntityImpl CreateEntity(Optional<TemplateAccessor> templateAccessor) => 
            new EntityImpl(this.engineServiceInternal, this.id.Value, this.name, templateAccessor);

        private Optional<TemplateAccessor> CreateTemplateAccessor() => 
            (this.templateDescription == null) ? Optional<TemplateAccessor>.empty() : ((this.configPath != null) ? Optional<TemplateAccessor>.of(new TemplateAccessor(this.templateDescription, this.configPath)) : Optional<TemplateAccessor>.of(new TemplateAccessor(this.templateDescription, this.templateYamlNode)));

        public EntityBuilder MarkAsPersistent() => 
            this;

        private void ResolveConfigPathByTemplate(Optional<TemplateAccessor> templateAccessor)
        {
            if (templateAccessor.IsPresent())
            {
                TemplateAccessor accessor = templateAccessor.Get();
                if (accessor.HasConfigPath())
                {
                    TemplateDescription templateDescription = accessor.TemplateDescription;
                    if (templateDescription.IsOverridedConfigPath())
                    {
                        accessor.ConfigPath = templateDescription.OverrideConfigPath(accessor.ConfigPath);
                    }
                }
            }
        }

        public EntityBuilder SetConfig(string configPath)
        {
            this.configPath = configPath;
            return this;
        }

        public EntityBuilder SetId(long id)
        {
            this.id = new long?(id);
            return this;
        }

        public EntityBuilder SetName(string name)
        {
            this.name = name;
            return this;
        }

        public EntityBuilder SetTemplate(TemplateDescription templateInfo)
        {
            this.templateDescription = templateInfo;
            return this;
        }

        public EntityBuilder SetTemplate(Type templateType)
        {
            this.SetTemplate(this.templateRegistry.GetTemplateInfo(templateType));
            return this;
        }

        public EntityBuilder SetTemplateAccessor(Optional<TemplateAccessor> templateAccessor)
        {
            this.templateAccessor = templateAccessor;
            return this;
        }

        public EntityBuilder SetTemplateYamlNode(YamlNode yamlNode)
        {
            this.templateYamlNode = yamlNode;
            return this;
        }
    }
}

