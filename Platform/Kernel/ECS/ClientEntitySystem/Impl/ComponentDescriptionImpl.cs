namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class ComponentDescriptionImpl : ComponentDescription
    {
        private readonly IDictionary<Type, ComponentInfo> infos = new Dictionary<Type, ComponentInfo>();
        private readonly MethodInfo componentMethod;
        private readonly string fieldName;
        private readonly Type componentType;

        public ComponentDescriptionImpl(MethodInfo componentMethod)
        {
            this.fieldName = componentMethod.Name;
            this.componentMethod = componentMethod;
            this.componentType = this._getComponentType(componentMethod);
        }

        private Type _getComponentType(MethodInfo componentMethod)
        {
            Type returnType = componentMethod.ReturnType;
            if (!typeof(Component).IsAssignableFrom(returnType))
            {
                throw new WrongComponentTypeException(returnType);
            }
            return returnType;
        }

        public void CollectInfo(ICollection<ComponentInfoBuilder> builders)
        {
            foreach (ComponentInfoBuilder builder in builders)
            {
                if (builder.IsAcceptable(this.componentMethod))
                {
                    this.infos[builder.TemplateComponentInfoClass] = builder.Build(this.componentMethod, this);
                }
            }
        }

        public T GetInfo<T>() where T: ComponentInfo
        {
            Type key = typeof(T);
            if (!this.infos.ContainsKey(key))
            {
                throw new ComponentInfoNotFoundException(key, this.componentMethod);
            }
            return (T) this.infos[key];
        }

        public bool IsInfoPresent(Type infoType) => 
            this.infos.ContainsKey(infoType);

        public string FieldName =>
            this.fieldName;

        public Type ComponentType =>
            this.componentType;
    }
}

