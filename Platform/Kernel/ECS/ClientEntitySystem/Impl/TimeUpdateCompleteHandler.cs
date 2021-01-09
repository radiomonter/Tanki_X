namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class TimeUpdateCompleteHandler : EventCompleteHandler
    {
        public TimeUpdateCompleteHandler(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) : base(typeof(TimeUpdateEvent), method, methodHandle, handlerArgumentsDescription)
        {
        }
    }
}

