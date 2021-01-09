namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public abstract class ComponentEvent : Event
    {
        protected ComponentEvent(Type componentType)
        {
            this.ComponentType = componentType;
        }

        public Type ComponentType { get; private set; }
    }
}

