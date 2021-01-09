namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class AfterFixedUpdateEventCompleteHandlerFactory : BroadcastEventHandlerFactory
    {
        public AfterFixedUpdateEventCompleteHandlerFactory() : base(typeof(OnEventComplete), typeof(AfterFixedUpdateEvent))
        {
        }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) => 
            new AfterFixedUpdateEventCompleteHandler(method, methodHandle, handlerArgumentsDescription);
    }
}

