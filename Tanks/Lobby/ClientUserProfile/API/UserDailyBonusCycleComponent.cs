namespace Tanks.Lobby.ClientUserProfile.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x8d5281ebf026ae2L)]
    public class UserDailyBonusCycleComponent : Component
    {
        public long CycleNumber { get; set; }
    }
}

