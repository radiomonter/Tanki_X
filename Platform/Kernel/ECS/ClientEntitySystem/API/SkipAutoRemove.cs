namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class SkipAutoRemove : Attribute
    {
    }
}

