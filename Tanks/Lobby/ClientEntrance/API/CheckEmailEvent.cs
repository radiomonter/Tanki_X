namespace Tanks.Lobby.ClientEntrance.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d33162e01105fcL)]
    public class CheckEmailEvent : Event
    {
        public CheckEmailEvent()
        {
        }

        public CheckEmailEvent(string email)
        {
            this.Email = email;
        }

        public CheckEmailEvent(string email, bool includeUnconfirmed)
        {
            this.Email = email;
            this.IncludeUnconfirmed = includeUnconfirmed;
        }

        public string Email { get; set; }

        public bool IncludeUnconfirmed { get; set; }
    }
}

