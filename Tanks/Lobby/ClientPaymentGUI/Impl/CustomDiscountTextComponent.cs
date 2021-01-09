namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;

    [Shared, SerialVersionUID(0x8d4f6b5c80e98c3L)]
    public class CustomDiscountTextComponent : Component
    {
        public Dictionary<string, string> LocalizedText;
        public Dictionary<string, string> SteamLocalizedText;

        public string Get(string local, bool steam)
        {
            Dictionary<string, string> dictionary = !steam ? this.LocalizedText : this.SteamLocalizedText;
            return (!dictionary.ContainsKey(local) ? string.Empty : dictionary[local].Replace(@"\n", "\n"));
        }
    }
}

