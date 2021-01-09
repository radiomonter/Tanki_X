namespace Tanks.Lobby.ClientPaymentGUI.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class SelectCountryEvent : Event
    {
        public string CountryName { get; set; }

        public string CountryCode { get; set; }
    }
}

