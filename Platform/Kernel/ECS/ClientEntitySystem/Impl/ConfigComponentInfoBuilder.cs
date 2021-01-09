namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Reflection;

    public class ConfigComponentInfoBuilder : ComponentInfoBuilder
    {
        public ComponentInfo Build(MethodInfo componentMethod, ComponentDescriptionImpl componentDescription)
        {
            PersistentConfig config = (PersistentConfig) componentMethod.GetCustomAttributes(typeof(PersistentConfig), false)[0];
            string keyName = config.value;
            if (keyName.Length == 0)
            {
                keyName = componentDescription.FieldName;
            }
            return new ConfigComponentInfo(keyName, config.configOptional);
        }

        public bool IsAcceptable(MethodInfo componentMethod) => 
            componentMethod.GetCustomAttributes(typeof(PersistentConfig), false).Length == 1;

        Type ComponentInfoBuilder.TemplateComponentInfoClass =>
            typeof(ConfigComponentInfo);
    }
}

