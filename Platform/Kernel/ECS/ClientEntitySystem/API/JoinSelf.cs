namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
    public class JoinSelf : Attribute
    {
    }
}

