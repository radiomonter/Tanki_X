namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class NodeAddedFireHandlerFactory : ConcreteEventHandlerFactory
    {
        public NodeAddedFireHandlerFactory() : base(typeof(OnEventFire), typeof(NodeAddedEvent))
        {
        }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) => 
            new NodeAddedFireHandler(method, methodHandle, handlerArgumentsDescription);
    }
}

