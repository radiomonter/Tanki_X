namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class CannotRegisterTemplatePartAsTemplateException : Exception
    {
        public CannotRegisterTemplatePartAsTemplateException(Type templateClass) : base("templateClass=" + templateClass)
        {
        }
    }
}

