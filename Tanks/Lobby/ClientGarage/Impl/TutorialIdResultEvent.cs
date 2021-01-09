namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15e75a2e230L)]
    public class TutorialIdResultEvent : Event
    {
        public long Id { get; set; }

        public bool ActionExecuted { get; set; }

        public bool ActionSuccess { get; set; }
    }
}

