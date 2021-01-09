namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class TimeUpdateFireHandler : EventFireHandler
    {
        public TimeUpdateFireHandler(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) : base(typeof(TimeUpdateEvent), method, methodHandle, handlerArgumentsDescription)
        {
        }
    }
}

