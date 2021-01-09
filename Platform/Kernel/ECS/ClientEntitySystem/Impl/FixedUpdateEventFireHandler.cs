namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class FixedUpdateEventFireHandler : EventFireHandler
    {
        public FixedUpdateEventFireHandler(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) : base(typeof(FixedUpdateEvent), method, methodHandle, handlerArgumentsDescription)
        {
        }
    }
}

