namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SpecialOfferContentLocalizationComponent : Component
    {
        public string SpriteUid { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}

