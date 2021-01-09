namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x4275b039a7ebef65L)]
    public class UserInBattleAsSpectatorComponent : Component
    {
        public long BattleId { get; set; }
    }
}

