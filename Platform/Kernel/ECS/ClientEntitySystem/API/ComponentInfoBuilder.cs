namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using System;
    using System.Reflection;

    public interface ComponentInfoBuilder
    {
        ComponentInfo Build(MethodInfo componentMethod, ComponentDescriptionImpl componentDescription);
        bool IsAcceptable(MethodInfo componentMethod);

        Type TemplateComponentInfoClass { get; }
    }
}

