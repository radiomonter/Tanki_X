namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ItemRarityComponent : Component
    {
        public ItemRarityType RarityType { get; set; }

        public bool NeedRarityFrame { get; set; }
    }
}

