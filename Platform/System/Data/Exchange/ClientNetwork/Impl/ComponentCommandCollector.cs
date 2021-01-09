namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.System.Data.Exchange.ClientNetwork.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ComponentCommandCollector : AbstractCommandCollector, ComponentListener
    {
        private readonly ComponentAndEventRegistrator componentAndEventRegistrator;
        private readonly SharedEntityRegistry entityRegistry;

        public ComponentCommandCollector(CommandCollector commandCollector, ComponentAndEventRegistrator componentAndEventRegistrator, SharedEntityRegistry entityRegistry) : base(commandCollector)
        {
            this.componentAndEventRegistrator = componentAndEventRegistrator;
            this.entityRegistry = entityRegistry;
        }

        private bool Allow(Entity entity, Type component) => 
            (this.entityRegistry.IsShared(entity.Id) && this.componentAndEventRegistrator.IsShared(component)) && !entity.HasComponent<DeletedEntityComponent>();

        public void OnComponentAdded(Entity entity, Component component)
        {
            if (this.Allow(entity, component.GetType()))
            {
                base.AddCommand(new ComponentAddCommand().Init(entity, component));
            }
        }

        public void OnComponentChanged(Entity entity, Component component)
        {
            if (!NetworkService.IsDecodeState && this.Allow(entity, component.GetType()))
            {
                base.AddCommand(new ComponentChangeCommand().Init(entity, component));
            }
        }

        public void OnComponentRemoved(Entity entity, Component component)
        {
            if (this.Allow(entity, component.GetType()))
            {
                base.AddCommand(new ComponentRemoveCommand().Init(entity, component.GetType()));
            }
        }

        [Inject]
        public static Platform.System.Data.Exchange.ClientNetwork.API.NetworkService NetworkService { get; set; }
    }
}

