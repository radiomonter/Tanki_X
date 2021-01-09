namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class TankOutlineMapEffectSwitchStateEvent : Event
    {
        public TankOutlineMapEffectSwitchStateEvent(Type stateType)
        {
            this.StateType = stateType;
        }

        public Type StateType { get; set; }
    }
}

