namespace Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x1527d0ba9efL)]
    public class ClientLocaleComponent : Component
    {
        public ClientLocaleComponent()
        {
        }

        public ClientLocaleComponent(string localeCode)
        {
            this.LocaleCode = localeCode;
        }

        public string LocaleCode { get; set; }
    }
}

