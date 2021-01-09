namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d51bfd53b96093L)]
    public class GiftPromoUIDataComponent : Component
    {
        public string Get(string local) => 
            !this.Texts.ContainsKey(local) ? string.Empty : this.Texts[local].Replace(@"\n", "\n");

        public string PromoKey { get; set; }

        public Dictionary<string, string> Texts { get; set; }
    }
}

