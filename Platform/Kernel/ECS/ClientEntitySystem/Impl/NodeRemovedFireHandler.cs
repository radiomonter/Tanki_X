namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class NodeRemovedFireHandler : EventFireHandler
    {
        public NodeRemovedFireHandler(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) : base(typeof(NodeRemoveEvent), method, methodHandle, handlerArgumentsDescription)
        {
        }
    }
}

