namespace Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d32e48ce6736bdL)]
    public class UnconfirmedUserEmailComponent : Component
    {
        public string Email { get; set; }
    }
}

