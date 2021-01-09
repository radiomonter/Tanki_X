namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class ComponentNotFoundInTemplateException : Exception
    {
        public ComponentNotFoundInTemplateException(Type componentClass, string templateName) : base("template= " + templateName + " component= " + componentClass.FullName)
        {
        }
    }
}

