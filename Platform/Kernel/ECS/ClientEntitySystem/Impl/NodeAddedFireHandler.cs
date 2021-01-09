namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class NodeAddedFireHandler : EventFireHandler
    {
        public NodeAddedFireHandler(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) : base(typeof(NodeAddedEvent), method, methodHandle, handlerArgumentsDescription)
        {
        }
    }
}

