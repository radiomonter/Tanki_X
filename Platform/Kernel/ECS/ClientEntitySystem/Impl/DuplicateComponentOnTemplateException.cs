namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class DuplicateComponentOnTemplateException : Exception
    {
        public DuplicateComponentOnTemplateException(TemplateDescription templateDescription, Type componentType) : base(string.Concat(objArray1))
        {
            object[] objArray1 = new object[] { "templateDescription=", templateDescription, " componentType=", componentType };
        }
    }
}

