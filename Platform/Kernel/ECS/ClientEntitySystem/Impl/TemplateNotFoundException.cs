namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class TemplateNotFoundException : Exception
    {
        public TemplateNotFoundException(long id) : base("templateId=" + id)
        {
        }
    }
}

