namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public abstract class BroadcastEventHandlerFactory : ConcreteEventHandlerFactory
    {
        protected BroadcastEventHandlerFactory(Type annotationEventClass, Type parameterClass) : base(annotationEventClass, parameterClass)
        {
        }

        protected override void Validate(MethodInfo method, HandlerArgumentsDescription argumentsDescription)
        {
            base.Validate(method, argumentsDescription);
            int num = 0;
            foreach (HandlerArgument argument in argumentsDescription.HandlerArguments)
            {
                if (argument.Context)
                {
                    num++;
                }
            }
            if (num > 1)
            {
                throw new MultipleContextNodesNotSupportedException(method);
            }
        }
    }
}

