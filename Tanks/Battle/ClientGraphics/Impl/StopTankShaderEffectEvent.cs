namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Runtime.InteropServices;

    public class StopTankShaderEffectEvent : BaseTankShaderEffectEvent
    {
        public StopTankShaderEffectEvent(string key, bool enableException = true) : base(key, enableException)
        {
        }
    }
}

