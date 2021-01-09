namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class EarlyUpdateCompleteHandler : EventCompleteHandler
    {
        public EarlyUpdateCompleteHandler(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) : base(typeof(EarlyUpdateEvent), method, methodHandle, handlerArgumentsDescription)
        {
        }
    }
}

