namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public class SystemRegistry
    {
        private readonly TemplateRegistry templateRegistry;
        private EngineServiceInternal engineService;
        private HashSet<Type> registeredSystemTypes;
        private NodeRegistrator nodeRegistrator;

        public SystemRegistry(TemplateRegistry templateRegistry, EngineServiceInternal engineService)
        {
            this.templateRegistry = templateRegistry;
            this.engineService = engineService;
            this.registeredSystemTypes = new HashSet<Type>();
            this.nodeRegistrator = new NodeRegistrator();
        }

        private void CheckDoubleRegistration(Type systemType)
        {
            if (this.registeredSystemTypes.Contains(systemType))
            {
                throw new SystemAlreadyRegisteredException(systemType);
            }
        }

        private void CollectHandlersAndNodes(ECSSystem systemInstance)
        {
            foreach (Handler handler in this.engineService.HandlerCollector.CollectHandlers(systemInstance))
            {
                IList<HandlerArgument> handlerArguments = handler.HandlerArgumentsDescription.HandlerArguments;
                foreach (HandlerArgument argument in handlerArguments)
                {
                    NodeDescriptionRegistry.AddNodeDescription(argument.NodeDescription);
                }
            }
        }

        private void DoRegister(ECSSystem system)
        {
            this.engineService.RequireInitState();
            this.RegisterNodesDeclaredInSystem(system.GetType());
            this.CollectHandlersAndNodes(system);
            this.InitSystem(system);
            this.registeredSystemTypes.Add(system.GetType());
        }

        public void ForceRegisterSystem(ECSSystem system)
        {
            this.DoRegister(system);
        }

        private void InitSystem(ECSSystem systemInstance)
        {
            systemInstance.Init(this.templateRegistry, this.engineService.DelayedEventManager, this.engineService, this.nodeRegistrator);
        }

        public void RegisterNode<T>() where T: Node
        {
            NodeDescriptionRegistry.AddNodeDescription(new StandardNodeDescription(typeof(T), null));
        }

        private void RegisterNodesDeclaredInSystem(Type systemClass)
        {
            foreach (Type type in systemClass.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Public))
            {
                if (type.IsSubclassOf(typeof(Node)) && !type.IsGenericTypeDefinition)
                {
                    NodeDescriptionRegistry.AddNodeDescription(new StandardNodeDescription(type, null));
                }
            }
        }

        public void RegisterSingleNode<T>() where T: Component
        {
            NodeDescriptionRegistry.AddNodeDescription(new StandardNodeDescription(typeof(SingleNode<T>), null));
        }

        public void RegisterSystem(ECSSystem system)
        {
            this.CheckDoubleRegistration(system.GetType());
            this.DoRegister(system);
        }

        [Inject]
        public static Platform.Kernel.ECS.ClientEntitySystem.API.NodeDescriptionRegistry NodeDescriptionRegistry { get; set; }
    }
}

