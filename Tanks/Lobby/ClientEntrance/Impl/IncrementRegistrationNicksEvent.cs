namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15282133d23L)]
    public class IncrementRegistrationNicksEvent : Event
    {
        public IncrementRegistrationNicksEvent()
        {
        }

        public IncrementRegistrationNicksEvent(string nick)
        {
            this.Nick = nick;
        }

        public string Nick { get; set; }
    }
}

