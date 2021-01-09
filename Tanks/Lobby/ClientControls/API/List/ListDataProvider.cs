namespace Tanks.Lobby.ClientControls.API.List
{
    using System;
    using System.Collections.Generic;

    public interface ListDataProvider
    {
        event Action<ListDataProvider> DataChanged;

        IList<object> Data { get; }

        object Selected { get; set; }
    }
}

