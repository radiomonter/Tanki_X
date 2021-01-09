namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class StarterPackMainElementComponent : Component
    {
        public long ItemId { get; set; }

        public string Title { get; set; }

        public int Count { get; set; }

        public string Description { get; set; }
    }
}

