namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class LocalDurationSystem : ECSSystem
    {
        [OnEventFire]
        public void Cancel(NodeRemoveEvent e, SingleNode<LocalDurationComponent> node)
        {
            if (!node.component.IsComplete)
            {
                node.component.LocalDurationExpireEvent.Cancel();
            }
        }

        [OnEventComplete]
        public void RemoveLocalDurationComponent(LocalDurationExpireEvent e, SingleNode<LocalDurationComponent> node)
        {
            node.component.IsComplete = true;
            node.Entity.RemoveComponent<LocalDurationComponent>();
        }

        [OnEventFire]
        public void ScheduleEventForDelete(NodeAddedEvent e, SingleNode<LocalDurationComponent> node)
        {
            LocalDurationComponent component = node.component;
            component.StartedTime = Date.Now;
            component.LocalDurationExpireEvent = base.NewEvent<LocalDurationExpireEvent>().Attach(node).ScheduleDelayed(node.component.Duration).Manager();
        }
    }
}

