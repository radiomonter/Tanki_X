namespace Tanks.Lobby.ClientSettings.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SoundListenerResourcesComponent : Component
    {
        public SoundListenerResourcesComponent(SoundListenerResourcesBehaviour resources)
        {
            this.Resources = resources;
        }

        public SoundListenerResourcesBehaviour Resources { get; set; }
    }
}

