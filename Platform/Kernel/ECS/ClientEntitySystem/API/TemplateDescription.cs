namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;
    using System.Collections.Generic;

    public interface TemplateDescription
    {
        ICollection<Type> GetAutoAddedComponentTypes();
        ComponentDescription GetComponentDescription(Type componentType);
        bool IsComponentDescriptionPresent(Type componentType);
        bool IsOverridedConfigPath();
        string OverrideConfigPath(string path);

        long TemplateId { get; }

        string TemplateName { get; }

        ICollection<ComponentDescription> ComponentDescriptions { get; }

        Type TemplateClass { get; }
    }
}

