namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
    public class Mandatory : Attribute
    {
    }
}

