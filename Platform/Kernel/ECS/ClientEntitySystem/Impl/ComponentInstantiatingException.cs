namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class ComponentInstantiatingException : Exception
    {
        public ComponentInstantiatingException(Type componentClass) : base(componentClass.FullName)
        {
        }
    }
}

