namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class DelayedEventTask : ScheduleManager
    {
        private readonly Event e;
        private readonly HashSet<Entity> entities;
        private readonly EngineServiceInternal engine;
        private bool canceled;
        private bool invoked;
        private double timeToExecute;
        [CompilerGenerated]
        private static Predicate<Entity> <>f__am$cache0;

        public DelayedEventTask(Event e, ICollection<Entity> entities, EngineServiceInternal engine, double timeToExecute)
        {
            this.e = e;
            this.entities = new HashSet<Entity>(entities);
            this.engine = engine;
            this.timeToExecute = timeToExecute;
        }

        public bool Cancel()
        {
            this.canceled = true;
            return this.invoked;
        }

        public bool IsCanceled() => 
            this.canceled;

        public void RemoveDeletedEntities()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = entity => !((EntityImpl) entity).Alive;
            }
            this.entities.RemoveWhere(<>f__am$cache0);
        }

        public bool Update(double time)
        {
            if (this.timeToExecute <= time)
            {
                Flow flow = this.engine.GetFlow();
                this.RemoveDeletedEntities();
                flow.SendEvent(this.e, this.entities);
                this.invoked = true;
            }
            return this.invoked;
        }
    }
}

