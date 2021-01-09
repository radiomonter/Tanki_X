namespace Platform.Library.ClientUnityIntegration.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using YamlDotNet.Serialization;

    public class ComponentInstanceDataUpdater
    {
        private static INamingConvention nameConvertor = new PascalToCamelCaseNamingConvertion();

        public static bool Update(EntityInternal entity, Component component, INamingConvention nameConvertor = null)
        {
            INamingConvention convention = ComponentInstanceDataUpdater.nameConvertor;
            if (!entity.TemplateAccessor.IsPresent())
            {
                return false;
            }
            TemplateAccessor accessor = entity.TemplateAccessor.Get();
            if (!accessor.HasConfigPath())
            {
                return false;
            }
            if (nameConvertor != null)
            {
                convention = nameConvertor;
            }
            Type componentType = component.GetType();
            if (!accessor.TemplateDescription.IsComponentDescriptionPresent(componentType))
            {
                return false;
            }
            ComponentDescription componentDescription = accessor.TemplateDescription.GetComponentDescription(componentType);
            if (!componentDescription.IsInfoPresent(typeof(ConfigComponentInfo)))
            {
                return false;
            }
            string keyName = componentDescription.GetInfo<ConfigComponentInfo>().KeyName;
            if (!accessor.YamlNode.HasValue(keyName))
            {
                return false;
            }
            UpdateComponentData(component, accessor.YamlNode.GetChildNode(keyName), convention);
            return true;
        }

        private static void UpdateComponentData(Component component, YamlNode componentYamlNode, INamingConvention nameConvertor)
        {
            foreach (PropertyInfo info in component.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                string key = nameConvertor.Apply(info.Name);
                if (componentYamlNode.HasValue(key) && info.CanWrite)
                {
                    info.SetValue(component, componentYamlNode.GetValue(key), null);
                }
            }
        }
    }
}

