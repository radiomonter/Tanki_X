namespace Tanks.Lobby.ClientBattleSelect.API
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Size=1)]
    public struct BattleEntry
    {
        public long Id { get; set; }
        public double Relevance { get; set; }
        public int FriendsInBattle { get; set; }
        public long Server { get; set; }
        public long LobbyServer { get; set; }
        public override string ToString() => 
            $"[BattleEntry Id={this.Id} Relevance={this.Relevance} FriendsInBattle={this.FriendsInBattle} Server={this.Server} Server={this.LobbyServer}]";
    }
}

