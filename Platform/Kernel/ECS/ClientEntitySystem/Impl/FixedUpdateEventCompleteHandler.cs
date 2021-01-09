namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class FixedUpdateEventCompleteHandler : EventCompleteHandler
    {
        public FixedUpdateEventCompleteHandler(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) : base(typeof(FixedUpdateEvent), method, methodHandle, handlerArgumentsDescription)
        {
        }
    }
}

