namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SpecialOfferScreenLocalizationComponent : Component
    {
        public string SpriteUid { get; set; }

        public string Footer { get; set; }

        public string Description { get; set; }
    }
}

