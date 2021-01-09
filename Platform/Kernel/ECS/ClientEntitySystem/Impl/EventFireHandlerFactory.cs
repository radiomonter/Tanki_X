namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class EventFireHandlerFactory : AbstractHandlerFactory
    {
        public EventFireHandlerFactory() : base(typeof(OnEventFire), Collections.SingletonList<Type>(typeof(Event)))
        {
        }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) => 
            new EventFireHandler(method.GetParameters()[0].ParameterType, method, methodHandle, handlerArgumentsDescription);
    }
}

