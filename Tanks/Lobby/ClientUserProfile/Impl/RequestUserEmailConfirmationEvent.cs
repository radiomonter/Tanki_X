﻿namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15374adeba1L)]
    public class RequestUserEmailConfirmationEvent : Event
    {
        public string Code { get; set; }
    }
}
