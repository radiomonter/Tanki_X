namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;

    public class TankShaderEffectAlreadyAddedException : ArgumentException
    {
        public TankShaderEffectAlreadyAddedException(string key) : base($"Key = [{key}]")
        {
        }
    }
}

