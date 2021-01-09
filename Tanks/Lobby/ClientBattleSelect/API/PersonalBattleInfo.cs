namespace Tanks.Lobby.ClientBattleSelect.API
{
    using System;
    using System.Runtime.CompilerServices;

    public class PersonalBattleInfo
    {
        public override string ToString() => 
            $"InLevelRange: {this.InLevelRange}, CanEnter: {this.CanEnter}";

        public bool InLevelRange { get; set; }

        public bool CanEnter { get; set; }
    }
}

