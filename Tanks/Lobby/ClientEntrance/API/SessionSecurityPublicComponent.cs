namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14f3a4bf47eL)]
    public class SessionSecurityPublicComponent : Component
    {
        public SessionSecurityPublicComponent()
        {
        }

        public SessionSecurityPublicComponent(string publicKey)
        {
            this.PublicKey = publicKey;
        }

        public string PublicKey { get; set; }
    }
}

