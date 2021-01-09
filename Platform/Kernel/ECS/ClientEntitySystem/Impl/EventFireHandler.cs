namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class EventFireHandler : Handler
    {
        public EventFireHandler(Type eventType, MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) : base(EventPhase.Fire, eventType, method, methodHandle, handlerArgumentsDescription)
        {
        }
    }
}

