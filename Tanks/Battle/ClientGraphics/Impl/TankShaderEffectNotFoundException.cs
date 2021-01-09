namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;

    public class TankShaderEffectNotFoundException : ArgumentException
    {
        public TankShaderEffectNotFoundException(string key) : base($"Key = [{key}]")
        {
        }
    }
}

