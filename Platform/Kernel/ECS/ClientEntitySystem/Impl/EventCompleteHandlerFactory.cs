namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public class EventCompleteHandlerFactory : AbstractHandlerFactory
    {
        public EventCompleteHandlerFactory() : base(typeof(OnEventComplete), Collections.SingletonList<Type>(typeof(Event)))
        {
        }

        protected override Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription) => 
            new EventCompleteHandler(method.GetParameters()[0].ParameterType, method, methodHandle, handlerArgumentsDescription);
    }
}

