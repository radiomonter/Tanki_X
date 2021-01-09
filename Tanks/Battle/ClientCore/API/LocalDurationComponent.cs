namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x8d2e6e146e9befaL)]
    public class LocalDurationComponent : Component
    {
        public LocalDurationComponent()
        {
        }

        public LocalDurationComponent(float duration)
        {
            this.Duration = duration;
        }

        public Date StartedTime { get; set; }

        public float Duration { get; set; }

        public bool IsComplete { get; set; }

        public ScheduleManager LocalDurationExpireEvent { get; set; }
    }
}

