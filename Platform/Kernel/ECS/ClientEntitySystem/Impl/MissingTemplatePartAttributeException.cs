namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class MissingTemplatePartAttributeException : Exception
    {
        public MissingTemplatePartAttributeException(Type templatePartClass) : base("class=" + templatePartClass)
        {
        }
    }
}

