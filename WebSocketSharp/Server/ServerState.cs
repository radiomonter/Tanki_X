namespace WebSocketSharp.Server
{
    using System;

    internal enum ServerState
    {
        Ready,
        Start,
        ShuttingDown,
        Stop
    }
}

