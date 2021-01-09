namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Reflection;

    public class AnnotationComponentInfoBuilder<T> : ComponentInfoBuilder where T: ComponentInfo, new()
    {
        private readonly Type annotationType;

        public AnnotationComponentInfoBuilder(Type annotationType, Type componentInfoClass)
        {
            this.annotationType = annotationType;
        }

        public ComponentInfo Build(MethodInfo componentMethod, ComponentDescriptionImpl componentDescription) => 
            Activator.CreateInstance<T>();

        public bool IsAcceptable(MethodInfo componentMethod) => 
            componentMethod.GetCustomAttributes(this.annotationType, true).Length == 1;

        public Type TemplateComponentInfoClass =>
            typeof(T);
    }
}

