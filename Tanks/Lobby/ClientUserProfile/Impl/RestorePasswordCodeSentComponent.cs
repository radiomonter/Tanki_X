namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x158671d2eaaL)]
    public class RestorePasswordCodeSentComponent : Component
    {
        public string Email { get; set; }
    }
}

