namespace Tanks.Battle.ClientMapEditor.Impl
{
    using System;
    using Tanks.Battle.ClientCore.API;

    public class SpawnPointBehaviour : EditorBehavior
    {
        public BattleMode battleMode;
        public TeamColor teamColor;

        public void Initialize(BattleMode battleMode, TeamColor teamColor)
        {
            this.battleMode = battleMode;
            this.teamColor = teamColor;
        }
    }
}

