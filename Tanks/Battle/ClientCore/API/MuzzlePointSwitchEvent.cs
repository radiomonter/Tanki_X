namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(-2650671245931951659L)]
    public class MuzzlePointSwitchEvent : Event
    {
        public MuzzlePointSwitchEvent()
        {
        }

        public MuzzlePointSwitchEvent(int index)
        {
            this.Index = index;
        }

        public int Index { get; set; }
    }
}

