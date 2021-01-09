namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class CurrentScreenComponent : Component
    {
        public ShowScreenData showScreenData;

        public Entity screen { get; set; }
    }
}

