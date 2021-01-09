namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;

    public class ConfigComponentConstructor : AbstractTemplateComponentConstructor
    {
        protected internal override Component GetComponentInstance(ComponentDescription componentDescription, EntityInternal entity)
        {
            Component component2;
            YamlNode yamlNode = entity.TemplateAccessor.Get().YamlNode;
            ConfigComponentInfo info = componentDescription.GetInfo<ConfigComponentInfo>();
            string keyName = info.KeyName;
            if (info.ConfigOptional && !yamlNode.HasValue(keyName))
            {
                return (Component) Activator.CreateInstance(componentDescription.ComponentType);
            }
            try
            {
                component2 = (Component) yamlNode.GetChildNode(keyName).ConvertTo(componentDescription.ComponentType);
            }
            catch (Exception exception)
            {
                TemplateAccessor accessor;
                string str2 = !accessor.HasConfigPath() ? yamlNode.ToString() : accessor.ConfigPath;
                object[] objArray1 = new object[] { "Error deserializing component ", componentDescription.ComponentType, " from configs, entity=", entity, ", key=", keyName, ", pathOrNode=", str2 };
                throw new Exception(string.Concat(objArray1), exception);
            }
            return component2;
        }

        protected override bool IsAcceptable(ComponentDescription componentDescription, EntityInternal entity) => 
            componentDescription.IsInfoPresent(typeof(ConfigComponentInfo));
    }
}

