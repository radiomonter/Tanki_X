namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SettingsChangedEvent<T> : Event where T: Component
    {
        public SettingsChangedEvent()
        {
        }

        public SettingsChangedEvent(T data)
        {
            this.Data = data;
        }

        public T Data { get; set; }
    }
}

