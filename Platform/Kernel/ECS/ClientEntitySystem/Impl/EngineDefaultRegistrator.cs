namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class EngineDefaultRegistrator
    {
        private readonly EngineServiceImpl engineServiceImpl;

        public EngineDefaultRegistrator(EngineServiceImpl engineServiceImpl)
        {
            this.engineServiceImpl = engineServiceImpl;
        }

        public void Register()
        {
            this.RegisterComponentConstructor();
            this.RegisterHandlerFactory();
            this.RegisterTasks();
            this.RegisterSystems();
        }

        private void RegisterComponentConstructor()
        {
            this.engineServiceImpl.RegisterComponentConstructor(new ConfigComponentConstructor());
        }

        private void RegisterHandlerFactory()
        {
            this.engineServiceImpl.RegisterHandlerFactory(new TimeUpdateFireHandlerFactory());
            this.engineServiceImpl.RegisterHandlerFactory(new TimeUpdateCompleteHandlerFactory());
            this.engineServiceImpl.RegisterHandlerFactory(new EarlyUpdateFireHandlerFactory());
            this.engineServiceImpl.RegisterHandlerFactory(new EarlyUpdateCompleteHandlerFactory());
            this.engineServiceImpl.RegisterHandlerFactory(new UpdateEventFireHandlerFactory());
            this.engineServiceImpl.RegisterHandlerFactory(new UpdateEventCompleteHandlerFactory());
            this.engineServiceImpl.RegisterHandlerFactory(new FixedUpdateEventFireHandlerFactory());
            this.engineServiceImpl.RegisterHandlerFactory(new FixedUpdateEventCompleteHandlerFactory());
            this.engineServiceImpl.RegisterHandlerFactory(new AfterFixedUpdateEventFireHandlerFactory());
            this.engineServiceImpl.RegisterHandlerFactory(new AfterFixedUpdateEventCompleteHandlerFactory());
            this.engineServiceImpl.RegisterHandlerFactory(new NodeAddedFireHandlerFactory());
            this.engineServiceImpl.RegisterHandlerFactory(new NodeAddedCompleteHandlerFactory());
            this.engineServiceImpl.RegisterHandlerFactory(new NodeRemovedFireHandlerFactory());
            this.engineServiceImpl.RegisterHandlerFactory(new NodeRemovedCompleteHandlerFactory());
            this.engineServiceImpl.RegisterHandlerFactory(new EventFireHandlerFactory());
            this.engineServiceImpl.RegisterHandlerFactory(new EventCompleteHandlerFactory());
        }

        private void RegisterSystems()
        {
            this.engineServiceImpl.RegisterSystem(new AutoAddComponentsSystem());
            this.engineServiceImpl.RegisterSystem(new AutoRemoveComponentsSystem(new AutoRemoveComponentsRegistryImpl(this.engineServiceImpl)));
        }

        private void RegisterTasks()
        {
            this.engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(TimeUpdateFireHandler));
            this.engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(TimeUpdateCompleteHandler));
            this.engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(EarlyUpdateFireHandler));
            this.engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(EarlyUpdateCompleteHandler));
            this.engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(UpdateEventFireHandler));
            this.engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(UpdateEventCompleteHandler));
            this.engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(UpdateEventFireHandler));
            this.engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(UpdateEventCompleteHandler));
            this.engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(FixedUpdateEventFireHandler));
            this.engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(FixedUpdateEventCompleteHandler));
            this.engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(AfterFixedUpdateEventFireHandler));
            this.engineServiceImpl.BroadcastEventHandlerCollector.Register(typeof(AfterFixedUpdateEventCompleteHandler));
            this.engineServiceImpl.RegisterTasksForHandler(typeof(TimeUpdateFireHandler));
            this.engineServiceImpl.RegisterTasksForHandler(typeof(TimeUpdateCompleteHandler));
            this.engineServiceImpl.RegisterTasksForHandler(typeof(EarlyUpdateFireHandler));
            this.engineServiceImpl.RegisterTasksForHandler(typeof(EarlyUpdateCompleteHandler));
            this.engineServiceImpl.RegisterTasksForHandler(typeof(UpdateEventFireHandler));
            this.engineServiceImpl.RegisterTasksForHandler(typeof(UpdateEventCompleteHandler));
            this.engineServiceImpl.RegisterTasksForHandler(typeof(FixedUpdateEventFireHandler));
            this.engineServiceImpl.RegisterTasksForHandler(typeof(FixedUpdateEventCompleteHandler));
            this.engineServiceImpl.RegisterTasksForHandler(typeof(AfterFixedUpdateEventFireHandler));
            this.engineServiceImpl.RegisterTasksForHandler(typeof(AfterFixedUpdateEventCompleteHandler));
            this.engineServiceImpl.RegisterTasksForHandler(typeof(NodeAddedFireHandler));
            this.engineServiceImpl.RegisterTasksForHandler(typeof(NodeAddedCompleteHandler));
            this.engineServiceImpl.RegisterTasksForHandler(typeof(NodeRemovedFireHandler));
            this.engineServiceImpl.RegisterTasksForHandler(typeof(NodeRemovedCompleteHandler));
            this.engineServiceImpl.RegisterTasksForHandler(typeof(EventFireHandler));
            this.engineServiceImpl.RegisterTasksForHandler(typeof(EventCompleteHandler));
        }
    }
}

