namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class TimeUpdateCompleteHandlerFactory : BroadcastEventHandlerFactory
    {
        public TimeUpdateCompleteHandlerFactory() : base(typeof(OnEventComplete), typeof(TimeUpdateEvent))
        {
        }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) => 
            new TimeUpdateCompleteHandler(method, methodHandle, handlerArgumentsDescription);
    }
}

