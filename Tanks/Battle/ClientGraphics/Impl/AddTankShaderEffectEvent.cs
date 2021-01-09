namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using System.Runtime.InteropServices;

    public class AddTankShaderEffectEvent : BaseTankShaderEffectEvent
    {
        public AddTankShaderEffectEvent(string key, bool enableException = false) : base(key, enableException)
        {
        }
    }
}

