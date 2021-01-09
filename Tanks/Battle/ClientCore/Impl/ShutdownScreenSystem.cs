namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;

    public class ShutdownScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void CheckShutdown(PeriodicShutdownCheckEvent e, SingleNode<ServerShutdownComponent> shutdown)
        {
            if ((shutdown.component.StopDateForClient.UnityTime - Date.Now.UnityTime) < 1f)
            {
                base.ScheduleEvent<TechnicalWorkEvent>(shutdown);
            }
            else
            {
                base.NewEvent(new PeriodicShutdownCheckEvent()).Attach(shutdown).ScheduleDelayed(1f);
            }
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, SingleNode<ServerShutdownComponent> shutdown)
        {
            if ((shutdown.component.StopDateForClient.UnityTime - Date.Now.UnityTime) > 1f)
            {
                base.NewEvent(new PeriodicShutdownCheckEvent()).Attach(shutdown).ScheduleDelayed(1f);
            }
            else
            {
                base.ScheduleEvent<TechnicalWorkEvent>(shutdown);
            }
        }
    }
}

