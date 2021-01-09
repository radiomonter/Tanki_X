namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using System.Collections.Generic;

    public class EventBuilderImpl : EventBuilder
    {
        private List<Entity> entities = new List<Entity>();
        private Flow flow;
        private Event eventInstance;
        private DelayedEventManager delayedEventManager;

        public EventBuilder Attach(Entity entity)
        {
            if (entity == null)
            {
                throw new NullEntityException();
            }
            this.entities.Add(entity);
            return this;
        }

        public EventBuilder Attach(Node node) => 
            this.Attach(node.Entity);

        public EventBuilder Attach<T>(ICollection<T> nodes) where T: Node
        {
            Collections.Enumerator<T> enumerator = Collections.GetEnumerator<T>(nodes);
            while (enumerator.MoveNext())
            {
                T current = enumerator.Current;
                this.Attach(current.Entity);
            }
            return this;
        }

        public EventBuilder AttachAll(ICollection<Entity> entities)
        {
            Collections.Enumerator<Entity> enumerator = Collections.GetEnumerator<Entity>(entities);
            while (enumerator.MoveNext())
            {
                this.Attach(enumerator.Current);
            }
            return this;
        }

        public EventBuilder AttachAll(params Entity[] entities)
        {
            for (int i = 0; i < entities.Length; i++)
            {
                this.Attach(entities[i]);
            }
            return this;
        }

        public EventBuilder AttachAll(params Node[] nodes)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                this.Attach(nodes[i]);
            }
            return this;
        }

        public EventBuilderImpl Init(DelayedEventManager delayedEventManager, Flow flow, Event eventInstance)
        {
            this.delayedEventManager = delayedEventManager;
            this.flow = flow;
            this.eventInstance = eventInstance;
            this.entities.Clear();
            return this;
        }

        public void Schedule()
        {
            this.flow.SendEvent(this.eventInstance, this.entities);
        }

        public ScheduledEvent ScheduleDelayed(float timeInSec) => 
            new ScheduledEventImpl(this.eventInstance, this.delayedEventManager.ScheduleDelayedEvent(this.eventInstance, this.entities, timeInSec));

        public ScheduledEvent SchedulePeriodic(float timeInSec) => 
            new ScheduledEventImpl(this.eventInstance, this.delayedEventManager.SchedulePeriodicEvent(this.eventInstance, this.entities, timeInSec));
    }
}

