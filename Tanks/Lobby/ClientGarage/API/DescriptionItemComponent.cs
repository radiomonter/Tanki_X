namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class DescriptionItemComponent : Component
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Flavor { get; set; }
    }
}

