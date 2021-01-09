namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AutoRemoveComponentsRegistryImpl : AutoRemoveComponentsRegistry, EngineHandlerRegistrationListener
    {
        private HashSet<Type> componentTypes = new HashSet<Type>();

        public AutoRemoveComponentsRegistryImpl(EngineService engineService)
        {
            engineService.AddSystemProcessingListener(this);
        }

        public bool IsComponentAutoRemoved(Type componentType) => 
            this.componentTypes.Contains(componentType);

        public bool IsNodeAutoRemoved(ICollection<Type> components) => 
            components.Any<Type>(new Func<Type, bool>(this.componentTypes.Contains));

        public void OnHandlerAdded(Handler handler)
        {
            if (ReferenceEquals(handler.EventType, typeof(NodeRemoveEvent)))
            {
                foreach (HandlerArgument argument in handler.ContextArguments)
                {
                    ICollection<Type> components = argument.NodeDescription.Components;
                    if (!this.IsNodeAutoRemoved(components))
                    {
                        this.RegisterOneComponent(components);
                    }
                }
            }
        }

        private void RegisterOneComponent(ICollection<Type> components)
        {
            foreach (Type type in components)
            {
                if (!type.IsDefined(typeof(SkipAutoRemove), true))
                {
                    this.componentTypes.Add(type);
                    break;
                }
            }
        }
    }
}

