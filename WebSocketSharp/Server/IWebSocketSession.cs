﻿namespace WebSocketSharp.Server
{
    using System;
    using WebSocketSharp;
    using WebSocketSharp.Net.WebSockets;

    public interface IWebSocketSession
    {
        WebSocketContext Context { get; }

        string ID { get; }

        string Protocol { get; }

        DateTime StartTime { get; }

        WebSocketState State { get; }
    }
}

