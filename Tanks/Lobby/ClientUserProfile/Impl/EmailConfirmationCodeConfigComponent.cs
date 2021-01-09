namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x162a90565e3L)]
    public class EmailConfirmationCodeConfigComponent : Component
    {
        public long EmailSendThresholdInSeconds { get; set; }

        public long ConfirmationCodeInputMaxLength { get; set; }
    }
}

