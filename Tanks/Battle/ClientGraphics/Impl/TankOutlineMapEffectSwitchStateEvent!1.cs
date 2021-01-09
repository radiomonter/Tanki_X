namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;

    public class TankOutlineMapEffectSwitchStateEvent<T> : TankOutlineMapEffectSwitchStateEvent where T: Node
    {
        public TankOutlineMapEffectSwitchStateEvent() : base(typeof(T))
        {
        }
    }
}

