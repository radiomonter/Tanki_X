namespace Tanks.Lobby.ClientEntrance.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14f2ac0217cL)]
    public class PersonalPasscodeEvent : Event
    {
        public PersonalPasscodeEvent()
        {
        }

        public PersonalPasscodeEvent(string passcode)
        {
            this.Passcode = passcode;
        }

        public string Passcode { get; set; }
    }
}

