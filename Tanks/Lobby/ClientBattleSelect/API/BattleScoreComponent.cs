namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x14e77fe14fbL)]
    public class BattleScoreComponent : Component
    {
        public int Score { get; set; }

        public int ScoreRed { get; set; }

        public int ScoreBlue { get; set; }
    }
}

