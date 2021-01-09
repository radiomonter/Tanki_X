namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class UpdateEventCompleteHandler : EventCompleteHandler
    {
        public UpdateEventCompleteHandler(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) : base(typeof(UpdateEvent), method, methodHandle, handlerArgumentsDescription)
        {
        }
    }
}

