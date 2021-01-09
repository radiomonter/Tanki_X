namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class EndSeasonRewardItem
    {
        public long StartPlace { get; set; }

        public long EndPlace { get; set; }

        [ProtocolOptional]
        public List<DroppedItem> Items { get; set; }
    }
}

