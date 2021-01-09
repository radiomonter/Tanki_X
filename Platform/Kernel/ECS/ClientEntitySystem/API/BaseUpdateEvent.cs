namespace Platform.Kernel.ECS.ClientEntitySystem.API
{
    using System;
    using System.Runtime.CompilerServices;

    public abstract class BaseUpdateEvent : Event
    {
        protected BaseUpdateEvent()
        {
        }

        public float DeltaTime { get; set; }
    }
}

