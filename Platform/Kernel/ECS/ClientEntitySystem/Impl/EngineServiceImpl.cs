namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class EngineServiceImpl : EngineServiceInternal, EngineService
    {
        protected readonly Flow flow;
        protected readonly TemplateRegistry templateRegistry;
        private readonly bool instanceFieldsInitialized;
        private EngineDefaultRegistrator engineDefaultRegistrator;
        private Platform.Kernel.ECS.ClientEntitySystem.Impl.SystemRegistry systemRegistry;

        public EngineServiceImpl(TemplateRegistry templateRegistry, Platform.Kernel.ECS.ClientEntitySystem.Impl.HandlerCollector handlerCollector, Platform.Kernel.ECS.ClientEntitySystem.Impl.EventMaker eventMaker, Platform.Kernel.ECS.ClientEntitySystem.API.ComponentBitIdRegistry componentBitIdRegistry)
        {
            this.templateRegistry = templateRegistry;
            if (!this.instanceFieldsInitialized)
            {
                this.InitializeInstanceFields();
                this.instanceFieldsInitialized = true;
            }
            this.HandlerCollector = handlerCollector;
            this.EventMaker = eventMaker;
            this.BroadcastEventHandlerCollector = new Platform.Kernel.ECS.ClientEntitySystem.Impl.BroadcastEventHandlerCollector(this.HandlerCollector);
            this.HandlerStateListener = new Platform.Kernel.ECS.ClientEntitySystem.Impl.HandlerStateListener(this.HandlerCollector);
            this.ComponentConstructors = new List<ComponentConstructor>();
            this.DelayedEventManager = new Platform.Kernel.ECS.ClientEntitySystem.Impl.DelayedEventManager(this);
            this.Engine = this.CreateDefaultEngine(this.DelayedEventManager);
            this.EntityRegistry = new EntityRegistryImpl();
            this.NodeCollector = new NodeCollectorImpl();
            this.systemRegistry = new Platform.Kernel.ECS.ClientEntitySystem.Impl.SystemRegistry(templateRegistry, this);
            this.NodeCache = new Platform.Kernel.ECS.ClientEntitySystem.Impl.NodeCache(this);
            this.ComponentBitIdRegistry = componentBitIdRegistry;
            this.HandlerContextDataStorage = new Platform.Kernel.ECS.ClientEntitySystem.Impl.HandlerContextDataStorage();
            this.FlowListeners = new HashSet<FlowListener>();
            this.ComponentListeners = new HashSet<ComponentListener>();
            this.EventListeners = new HashSet<EventListener>();
            this.EventInstancesStorageForReuse = new TypeInstancesStorage<Event>();
            this.engineDefaultRegistrator.Register();
            this.CollectEmptyEventInstancesForReuse();
            this.flow = new Flow(this);
        }

        public void AddComponentListener(ComponentListener componentListener)
        {
            this.ComponentListeners.Add(componentListener);
        }

        public void AddEventListener(EventListener eventListener)
        {
            this.EventListeners.Add(eventListener);
        }

        public void AddFlowListener(FlowListener flowListener)
        {
            this.FlowListeners.Add(flowListener);
        }

        public void AddSystemProcessingListener(EngineHandlerRegistrationListener listener)
        {
            this.HandlerCollector.AddHandlerListener(listener);
        }

        public void CollectEmptyEventInstancesForReuse()
        {
            List<Type> eventTypes = new List<Type>(0x200);
            AssemblyTypeCollector.CollectEmptyEventTypes(eventTypes);
            foreach (Type type in eventTypes)
            {
                this.EventInstancesStorageForReuse.AddInstance(type);
            }
        }

        private Platform.Kernel.ECS.ClientEntitySystem.API.Engine CreateDefaultEngine(Platform.Kernel.ECS.ClientEntitySystem.Impl.DelayedEventManager delayedEventManager)
        {
            EngineImpl impl = new EngineImpl();
            impl.Init(this.templateRegistry, delayedEventManager);
            return impl;
        }

        public virtual EntityBuilder CreateEntityBuilder() => 
            new EntityBuilder(this, this.EntityRegistry, this.templateRegistry);

        public void ExecuteInFlow(Consumer<Platform.Kernel.ECS.ClientEntitySystem.API.Engine> consumer)
        {
            Flow.Current.ScheduleWith(consumer);
        }

        public void ForceRegisterSystem(ECSSystem system)
        {
            this.systemRegistry.ForceRegisterSystem(system);
        }

        public Flow GetFlow() => 
            this.flow;

        private void InitializeInstanceFields()
        {
            this.engineDefaultRegistrator = new EngineDefaultRegistrator(this);
        }

        public Flow NewFlow()
        {
            this.RequireRunningState();
            return this.flow;
        }

        public void RegisterComponentConstructor(ComponentConstructor componentConstructor)
        {
            this.ComponentConstructors.Add(componentConstructor);
        }

        public void RegisterHandlerFactory(HandlerFactory factory)
        {
            this.HandlerCollector.RegisterHandlerFactory(factory);
        }

        public void RegisterSystem(ECSSystem system)
        {
            this.systemRegistry.RegisterSystem(system);
        }

        public void RegisterTasksForHandler(Type handlerType)
        {
            this.HandlerCollector.RegisterTasksForHandler(handlerType);
        }

        public void RemoveFlowListener(FlowListener flowListener)
        {
            this.FlowListeners.Remove(flowListener);
        }

        public virtual void RequireInitState()
        {
            if (this.IsRunning)
            {
                throw new RegistrationAfterStartECSException();
            }
        }

        private void RequireRunningState()
        {
            if (!this.IsRunning)
            {
                throw new ECSNotRunningException();
            }
        }

        public void RunECSKernel()
        {
            if (!this.IsRunning)
            {
                this.HandlerCollector.SortHandlers();
                this.IsRunning = true;
                this.EntityStub = new Platform.Kernel.ECS.ClientEntitySystem.Impl.EntityStub();
                this.EntityRegistry.RegisterEntity(this.EntityStub);
            }
        }

        public bool IsRunning { get; private set; }

        public ICollection<ComponentConstructor> ComponentConstructors { get; private set; }

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.HandlerCollector HandlerCollector { get; private set; }

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.EventMaker EventMaker { get; private set; }

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.BroadcastEventHandlerCollector BroadcastEventHandlerCollector { get; private set; }

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.DelayedEventManager DelayedEventManager { get; private set; }

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.EntityRegistry EntityRegistry { get; private set; }

        public virtual NodeCollectorImpl NodeCollector { get; protected set; }

        public Entity EntityStub { get; private set; }

        public Platform.Kernel.ECS.ClientEntitySystem.API.Engine Engine { get; private set; }

        public Platform.Kernel.ECS.ClientEntitySystem.API.ComponentBitIdRegistry ComponentBitIdRegistry { get; private set; }

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.NodeCache NodeCache { get; protected set; }

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.HandlerStateListener HandlerStateListener { get; private set; }

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.HandlerContextDataStorage HandlerContextDataStorage { get; private set; }

        public ICollection<FlowListener> FlowListeners { get; private set; }

        public ICollection<ComponentListener> ComponentListeners { get; private set; }

        public ICollection<EventListener> EventListeners { get; private set; }

        public TypeInstancesStorage<Event> EventInstancesStorageForReuse { get; private set; }

        public Platform.Kernel.ECS.ClientEntitySystem.Impl.SystemRegistry SystemRegistry =>
            this.systemRegistry;
    }
}

