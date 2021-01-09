namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x154c8416cd2L)]
    public class SetScoreTablePositionEvent : Event
    {
        public SetScoreTablePositionEvent()
        {
        }

        public SetScoreTablePositionEvent(int position)
        {
            this.Position = position;
        }

        public int Position { get; set; }
    }
}

