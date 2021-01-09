namespace Tanks.Battle.ClientCore.API
{
    using System;

    public class TankInvisibilityEffectSwitchStateEvent<T> : TankInvisibilityEffectSwitchStateEvent where T: Node
    {
        public TankInvisibilityEffectSwitchStateEvent() : base(typeof(T))
        {
        }
    }
}

