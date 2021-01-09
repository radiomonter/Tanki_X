namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class NodeRemovedCompleteHandler : EventCompleteHandler
    {
        public NodeRemovedCompleteHandler(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) : base(typeof(NodeRemoveEvent), method, methodHandle, handlerArgumentsDescription)
        {
        }
    }
}

