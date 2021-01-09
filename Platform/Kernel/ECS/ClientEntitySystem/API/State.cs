namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
    public class State : Attribute
    {
    }
}

