namespace Tanks.Battle.ClientMapEditor.Impl
{
    using System;
    using Tanks.Battle.ClientCore.API;

    public class FlagBehaviour : EditorBehavior
    {
        public TeamColor teamColor;

        public void SetTeamColor(TeamColor teamColor)
        {
            this.teamColor = teamColor;
        }
    }
}

