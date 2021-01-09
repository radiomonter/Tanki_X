namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public abstract class AbstractHandlerFactory : HandlerFactory
    {
        private readonly Type annotationEventClass;
        private readonly IList<Type> parameterClasses;

        protected internal AbstractHandlerFactory(Type annotationEventClass, IList<Type> parameterClasses)
        {
            this.annotationEventClass = annotationEventClass;
            this.parameterClasses = parameterClasses;
        }

        public Handler CreateHandler(MethodInfo method, ECSSystem system)
        {
            if (!this.IsSelf(method))
            {
                return null;
            }
            this.ValidateMethodIsPublic(method);
            this.ValidateEventTypes(method);
            HandlerArgumentsDescription argumentsDescription = new HandlerArgumentsParser(method).Parse();
            this.Validate(method, argumentsDescription);
            return this.CreateHandlerInstance(method, this.GetMethodHandle(method, system), argumentsDescription);
        }

        protected abstract Handler CreateHandlerInstance(MethodInfo method, MethodHandle methodHandle, HandlerArgumentsDescription handlerArgumentsDescription);
        public MethodHandle GetMethodHandle(MethodInfo method, ECSSystem system) => 
            new MethodHandle(method, system);

        protected virtual bool IsSelf(MethodInfo method) => 
            method.GetCustomAttributes(this.annotationEventClass, true).Length == 1;

        protected virtual void Validate(MethodInfo method, HandlerArgumentsDescription argumentsDescription)
        {
        }

        private void ValidateEventTypes(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            if (parameters.Length < this.parameterClasses.Count)
            {
                throw new EventAbsentInArgumentHandlerFactoryException(method);
            }
            for (int i = 0; i < this.parameterClasses.Count; i++)
            {
                if (!this.parameterClasses[i].IsAssignableFrom(parameters[i].ParameterType))
                {
                    throw new EventAbsentInArgumentHandlerFactoryException(method, this.parameterClasses[i]);
                }
            }
        }

        private void ValidateMethodIsPublic(MethodInfo method)
        {
            if (!method.IsPublic)
            {
                throw new HandlerNotPublicException(method);
            }
        }
    }
}

