namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class JoinToCollectionException : Exception
    {
        public JoinToCollectionException(MethodInfo method, HandlerArgument handlerArgument) : base($"Join-node can''t be joined by collection
[method={method},
handlerArgument={handlerArgument}]")
        {
        }
    }
}

