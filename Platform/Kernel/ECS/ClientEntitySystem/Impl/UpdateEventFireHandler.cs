namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class UpdateEventFireHandler : EventFireHandler
    {
        public UpdateEventFireHandler(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) : base(typeof(UpdateEvent), method, methodHandle, handlerArgumentsDescription)
        {
        }
    }
}

