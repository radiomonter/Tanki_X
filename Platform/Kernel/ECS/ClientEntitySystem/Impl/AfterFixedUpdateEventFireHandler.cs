namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class AfterFixedUpdateEventFireHandler : EventFireHandler
    {
        public AfterFixedUpdateEventFireHandler(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) : base(typeof(AfterFixedUpdateEvent), method, methodHandle, handlerArgumentsDescription)
        {
        }
    }
}

