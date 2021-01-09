namespace Tanks.Battle.ClientHUD.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class ShowMessageAfterKilledEvent : Event
    {
        public int killerRank;
        public TeamColor killerTeam;
        public long killerItem;

        public string KillerUserUid { get; set; }
    }
}

