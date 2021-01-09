namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x167b6963c03L)]
    public class RestrictionByUserFractionComponent : Component
    {
        public long FractionId { get; set; }
    }
}

