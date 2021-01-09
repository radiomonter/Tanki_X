namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class UpdateEventFireHandlerFactory : BroadcastEventHandlerFactory
    {
        public UpdateEventFireHandlerFactory() : base(typeof(OnEventFire), typeof(UpdateEvent))
        {
        }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) => 
            new UpdateEventFireHandler(method, methodHandle, handlerArgumentsDescription);
    }
}

