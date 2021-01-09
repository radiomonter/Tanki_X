namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class EarlyUpdateCompleteHandlerFactory : BroadcastEventHandlerFactory
    {
        public EarlyUpdateCompleteHandlerFactory() : base(typeof(OnEventComplete), typeof(EarlyUpdateEvent))
        {
        }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) => 
            new EarlyUpdateCompleteHandler(method, methodHandle, handlerArgumentsDescription);
    }
}

