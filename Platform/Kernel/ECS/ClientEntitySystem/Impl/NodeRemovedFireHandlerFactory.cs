namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class NodeRemovedFireHandlerFactory : ConcreteEventHandlerFactory
    {
        public NodeRemovedFireHandlerFactory() : base(typeof(OnEventFire), typeof(NodeRemoveEvent))
        {
        }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) => 
            new NodeRemovedFireHandler(method, methodHandle, handlerArgumentsDescription);
    }
}

