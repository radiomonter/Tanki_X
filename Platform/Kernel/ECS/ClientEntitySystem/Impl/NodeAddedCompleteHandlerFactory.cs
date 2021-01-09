namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class NodeAddedCompleteHandlerFactory : ConcreteEventHandlerFactory
    {
        public NodeAddedCompleteHandlerFactory() : base(typeof(OnEventComplete), typeof(NodeAddedEvent))
        {
        }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) => 
            new NodeAddedCompleteHandler(method, methodHandle, handlerArgumentsDescription);
    }
}

