namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;

    public class EntityCloner
    {
        private readonly Type newEntityComponentType = typeof(NewEntityComponent);

        public EntityInternal Clone(string name, EntityInternal entity, EntityBuilder entityBuilder)
        {
            entityBuilder.SetName(name);
            if (entity.TemplateAccessor.IsPresent())
            {
                TemplateAccessor accessor = entity.TemplateAccessor.Get();
                if (accessor != null)
                {
                    TemplateDescription templateDescription = accessor.TemplateDescription;
                    entityBuilder.SetTemplate(templateDescription.TemplateClass);
                    if (accessor.HasConfigPath())
                    {
                        entityBuilder.SetConfig(accessor.ConfigPath);
                    }
                    else
                    {
                        YamlNode yamlNode = accessor.YamlNode;
                        if (yamlNode != null)
                        {
                            entityBuilder.SetTemplateYamlNode(yamlNode);
                        }
                    }
                }
            }
            EntityInternal internal2 = entityBuilder.Build(true);
            foreach (Type type in entity.ComponentClasses)
            {
                if (!ReferenceEquals(type, this.newEntityComponentType))
                {
                    internal2.AddComponent(entity.GetComponent(type));
                }
            }
            return internal2;
        }
    }
}

