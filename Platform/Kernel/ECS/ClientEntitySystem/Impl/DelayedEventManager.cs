namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class DelayedEventManager
    {
        private readonly EngineServiceInternal engineService;
        private LinkedList<PeriodicEventTask> periodicTasks = new LinkedList<PeriodicEventTask>();
        private LinkedList<DelayedEventTask> delayedTasks = new LinkedList<DelayedEventTask>();

        public DelayedEventManager(EngineServiceInternal engine)
        {
            this.engineService = engine;
        }

        public ScheduleManager ScheduleDelayedEvent(Event e, ICollection<Entity> entities, float timeInSec)
        {
            DelayedEventTask task = new DelayedEventTask(e, entities, this.engineService, (double) (Time.time + timeInSec));
            this.delayedTasks.AddLast(task);
            return task;
        }

        public ScheduleManager SchedulePeriodicEvent(Event e, ICollection<Entity> entities, float timeInSec)
        {
            PeriodicEventTask task = new PeriodicEventTask(e, this.engineService, entities, timeInSec);
            this.periodicTasks.AddLast(task);
            return task;
        }

        private void TryUpdate(double time, DelayedEventTask task)
        {
            try
            {
                if (task.Update(time))
                {
                    this.delayedTasks.Remove(task);
                }
            }
            catch
            {
                this.delayedTasks.Remove(task);
                throw;
            }
        }

        public void Update(double time)
        {
            this.UpdatePeriodicTasks(time);
            this.UpdateDelayedTasks(time);
        }

        private void UpdateDelayedTasks(double time)
        {
            LinkedListNode<DelayedEventTask> next;
            for (LinkedListNode<DelayedEventTask> node = this.delayedTasks.First; node != null; node = next)
            {
                DelayedEventTask task = node.Value;
                next = node.Next;
                if (task.IsCanceled())
                {
                    this.delayedTasks.Remove(task);
                }
                else
                {
                    this.TryUpdate(time, task);
                }
            }
        }

        private void UpdatePeriodicTasks(double time)
        {
            LinkedListNode<PeriodicEventTask> next;
            for (LinkedListNode<PeriodicEventTask> node = this.periodicTasks.First; node != null; node = next)
            {
                PeriodicEventTask task = node.Value;
                next = node.Next;
                if (task.IsCanceled())
                {
                    this.periodicTasks.Remove(node);
                }
                else
                {
                    task.Update(time);
                }
            }
        }
    }
}

