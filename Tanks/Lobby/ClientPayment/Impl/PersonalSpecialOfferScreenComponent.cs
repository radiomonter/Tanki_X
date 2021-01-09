namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15f349f82f2L)]
    public class PersonalSpecialOfferScreenComponent : SharedChangeableComponent
    {
        public bool Shown { get; set; }

        public bool ShownWhenRemains { get; set; }
    }
}

