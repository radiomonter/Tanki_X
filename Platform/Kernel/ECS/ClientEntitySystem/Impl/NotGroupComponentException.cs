namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;

    public class NotGroupComponentException : Exception
    {
        public NotGroupComponentException(Type type) : base("Type: " + type)
        {
        }
    }
}

