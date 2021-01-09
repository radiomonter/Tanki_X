namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public abstract class BaseTankShaderEffectEvent : Event
    {
        protected BaseTankShaderEffectEvent(string key, bool enableException = false)
        {
            this.Key = key;
            this.EnableException = enableException;
        }

        public string Key { get; set; }

        public bool EnableException { get; set; }
    }
}

