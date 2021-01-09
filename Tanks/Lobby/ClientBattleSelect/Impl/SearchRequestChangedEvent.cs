namespace Tanks.Lobby.ClientBattleSelect.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;

    [Shared, SerialVersionUID(0x14fac0a0f0eL)]
    public class SearchRequestChangedEvent : Event
    {
        public SearchRequestChangedEvent()
        {
        }

        public SearchRequestChangedEvent(IndexRange range)
        {
            this.Range = range;
        }

        public IndexRange Range { get; set; }
    }
}

