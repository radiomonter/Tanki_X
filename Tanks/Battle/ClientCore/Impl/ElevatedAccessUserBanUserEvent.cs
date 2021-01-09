namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x15e0dcd30c1L)]
    public class ElevatedAccessUserBanUserEvent : ElevatedAccessUserBasePunishEvent
    {
        public string Type { get; set; }
    }
}

