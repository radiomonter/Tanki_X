namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class HandlerNotPublicException : Exception
    {
        public HandlerNotPublicException(MethodInfo method) : base("method=" + method)
        {
        }
    }
}

