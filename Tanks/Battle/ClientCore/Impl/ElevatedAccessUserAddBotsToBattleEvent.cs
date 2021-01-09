namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    [Shared, SerialVersionUID(0x160d9d0570fL)]
    public class ElevatedAccessUserAddBotsToBattleEvent : Event
    {
        public int Count { get; set; }

        public Tanks.Battle.ClientCore.API.TeamColor TeamColor { get; set; }
    }
}

