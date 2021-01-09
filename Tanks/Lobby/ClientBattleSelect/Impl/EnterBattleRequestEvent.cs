namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;

    [Shared, SerialVersionUID(-4518638013450931090L)]
    public class EnterBattleRequestEvent : Event
    {
        public EnterBattleRequestEvent()
        {
        }

        public EnterBattleRequestEvent(Tanks.Battle.ClientCore.API.TeamColor teamColor)
        {
            this.TeamColor = teamColor;
        }

        public Tanks.Battle.ClientCore.API.TeamColor TeamColor { get; set; }

        public string Source { get; set; }
    }
}

