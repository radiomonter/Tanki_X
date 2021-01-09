namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    public class ClientBattleParams
    {
        public override string ToString()
        {
            object[] args = new object[9];
            args[0] = this.BattleMode;
            args[1] = this.MapId;
            args[2] = this.MaxPlayers;
            args[3] = this.TimeLimit;
            args[4] = this.ScoreLimit;
            args[5] = this.FriendlyFire;
            args[6] = this.Gravity;
            args[7] = this.KillZoneEnabled;
            args[8] = this.DisabledModules;
            return string.Format("BattleMode: {0}, MapId: {1}, MaxPlayers: {2}, TimeLimit: {3}, ScoreLimit: {4}, FriendlyFire: {5}, Gravity: {6}, KillZoneEnabled: {7}, DisabledModules: {8}", args);
        }

        public Tanks.Battle.ClientCore.API.BattleMode BattleMode { get; set; }

        public long MapId { get; set; }

        public int MaxPlayers { get; set; }

        public int TimeLimit { get; set; }

        public int ScoreLimit { get; set; }

        public bool FriendlyFire { get; set; }

        public GravityType Gravity { get; set; }

        public bool KillZoneEnabled { get; set; }

        public bool DisabledModules { get; set; }
    }
}

