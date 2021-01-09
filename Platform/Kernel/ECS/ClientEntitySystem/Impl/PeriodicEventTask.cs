namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;

    public class PeriodicEventTask : ScheduleManager
    {
        private readonly Event e;
        private readonly EngineServiceInternal engineService;
        private readonly HashSet<Entity> contextEntities;
        private readonly float timeInSec;
        private double timeToExecute;
        private bool canceled;

        public PeriodicEventTask(Event e, EngineServiceInternal engineService, ICollection<Entity> contextEntities, float timeInSec)
        {
            this.e = e;
            this.engineService = engineService;
            this.contextEntities = new HashSet<Entity>(contextEntities);
            this.timeInSec = timeInSec;
            this.NewPeriod();
        }

        public bool Cancel()
        {
            if (this.canceled)
            {
                return false;
            }
            this.canceled = true;
            return true;
        }

        public bool IsCanceled() => 
            this.canceled;

        private void NewPeriod()
        {
            this.timeToExecute = this.timeInSec;
        }

        public void Update(double time)
        {
            while (this.timeToExecute <= time)
            {
                this.timeToExecute += this.timeInSec;
                this.engineService.GetFlow().SendEvent(this.e, this.contextEntities);
            }
        }
    }
}

