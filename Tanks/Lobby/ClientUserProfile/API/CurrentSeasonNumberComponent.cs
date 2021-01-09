namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15f4ce72a2dL)]
    public class CurrentSeasonNumberComponent : Component
    {
        public int SeasonNumber { get; set; }
    }
}

