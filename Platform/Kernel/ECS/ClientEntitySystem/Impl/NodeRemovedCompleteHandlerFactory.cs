namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class NodeRemovedCompleteHandlerFactory : ConcreteEventHandlerFactory
    {
        public NodeRemovedCompleteHandlerFactory() : base(typeof(OnEventComplete), typeof(NodeRemoveEvent))
        {
        }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) => 
            new NodeRemovedCompleteHandler(method, methodHandle, handlerArgumentsDescription);
    }
}

