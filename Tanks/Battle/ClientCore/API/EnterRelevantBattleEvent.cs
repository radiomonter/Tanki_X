namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x158054d62beL)]
    public class EnterRelevantBattleEvent : Event
    {
        public TeamColor PreferredTeam { get; set; }

        public long PreferredBattle { get; set; }

        public string Source { get; set; }
    }
}

