namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;

    public class EventCommandCollector : AbstractCommandCollector, EventListener
    {
        private readonly ComponentAndEventRegistrator componentAndEventRegistrator;
        private readonly SharedEntityRegistry entityRegistry;

        public EventCommandCollector(CommandCollector commandCollector, ComponentAndEventRegistrator componentAndEventRegistrator, SharedEntityRegistry entityRegistry) : base(commandCollector)
        {
            this.componentAndEventRegistrator = componentAndEventRegistrator;
            this.entityRegistry = entityRegistry;
        }

        public void OnEventSend(Event evt, ICollection<Entity> entities)
        {
            if (evt.GetType().IsDefined(typeof(Shared), true))
            {
                Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator<Entity>(entities);
                object[] instanceArray = AbstractCommandCollector.Cache.array.GetInstanceArray(entities.Count);
                int length = 0;
                while (enumerator.MoveNext())
                {
                    Entity current = enumerator.Current;
                    if (this.entityRegistry.IsShared(current.Id))
                    {
                        instanceArray[length++] = current;
                    }
                }
                if (length > 0)
                {
                    Entity[] destinationArray = AbstractCommandCollector.Cache.entityArray.GetInstanceArray(length);
                    Array.Copy(instanceArray, destinationArray, length);
                    base.AddCommand(new SendEventCommand().Init(destinationArray, evt));
                }
            }
        }
    }
}

