namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class FixedUpdateEventFireHandlerFactory : BroadcastEventHandlerFactory
    {
        public FixedUpdateEventFireHandlerFactory() : base(typeof(OnEventFire), typeof(FixedUpdateEvent))
        {
        }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) => 
            new FixedUpdateEventFireHandler(method, methodHandle, handlerArgumentsDescription);
    }
}

