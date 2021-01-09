namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class TankInvisibilityEffectSwitchStateEvent : Event
    {
        public TankInvisibilityEffectSwitchStateEvent(Type stateType)
        {
            this.StateType = stateType;
        }

        public Type StateType { get; set; }
    }
}

