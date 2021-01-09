namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class UnknownGroupContainerException : Exception
    {
        public UnknownGroupContainerException(Type containerClass) : base("containerClass=" + containerClass)
        {
        }
    }
}

