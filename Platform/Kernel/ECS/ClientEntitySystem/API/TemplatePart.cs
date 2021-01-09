namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class TemplatePart : Attribute
    {
    }
}

