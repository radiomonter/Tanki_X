namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using System;
    using System.Reflection;

    public abstract class ConcreteEventHandlerFactory : AbstractHandlerFactory
    {
        private readonly Type parameterClass;

        protected ConcreteEventHandlerFactory(Type annotationEventClass, Type parameterClass) : base(annotationEventClass, Collections.SingletonList<Type>(parameterClass))
        {
            this.parameterClass = parameterClass;
        }

        protected override bool IsSelf(MethodInfo method)
        {
            if (!base.IsSelf(method))
            {
                return false;
            }
            ParameterInfo[] parameters = method.GetParameters();
            return ((parameters.Length > 0) && ReferenceEquals(parameters[0].ParameterType, this.parameterClass));
        }
    }
}

