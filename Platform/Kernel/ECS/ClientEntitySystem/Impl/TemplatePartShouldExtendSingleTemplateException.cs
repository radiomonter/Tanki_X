namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class TemplatePartShouldExtendSingleTemplateException : Exception
    {
        public TemplatePartShouldExtendSingleTemplateException(Type templatePartClass) : base("templatePartClass=" + templatePartClass)
        {
        }

        public TemplatePartShouldExtendSingleTemplateException(Type templatePartClass, Type baseClass) : base(string.Concat(objArray1))
        {
            object[] objArray1 = new object[] { "templatePartClass=", templatePartClass, " baseClass=", baseClass };
        }
    }
}

