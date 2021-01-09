namespace WebSocketSharp.Net
{
    using System;

    internal enum InputChunkState
    {
        None,
        Data,
        DataEnded,
        Trailer,
        End
    }
}

