namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-5086569348607290080L)]
    public class ActivateTankEvent : Event
    {
        public ActivateTankEvent()
        {
        }

        public ActivateTankEvent(long phase)
        {
            this.Phase = phase;
        }

        public long Phase { get; set; }
    }
}

