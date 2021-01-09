namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;
    using System.Collections.Generic;

    public interface TemplateRegistry
    {
        ICollection<Type> GetParentTemplateClasses(Type templateType);
        TemplateDescription GetTemplateInfo(long id);
        TemplateDescription GetTemplateInfo(Type templateType);
        void Register<T>() where T: Template;
        void Register(Type templateClass);
        void RegisterComponentInfoBuilder(ComponentInfoBuilder componentInfoBuilder);
        void RegisterPart<T>() where T: Template;
        void RegisterPart(Type templatePartClass);

        ICollection<ComponentInfoBuilder> ComponentInfoBuilders { get; }
    }
}

