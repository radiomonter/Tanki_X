namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SwitchSoundListenerStateEvent : Event
    {
        public SwitchSoundListenerStateEvent(Type stateType)
        {
            this.StateType = stateType;
        }

        public Type StateType { get; set; }
    }
}

