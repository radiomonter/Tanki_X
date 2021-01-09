namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15e75a08359L)]
    public class ApplyTutorialIdEvent : Event
    {
        public ApplyTutorialIdEvent(long id)
        {
            this.Id = id;
        }

        public long Id { get; private set; }
    }
}

