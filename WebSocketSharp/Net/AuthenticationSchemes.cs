﻿namespace WebSocketSharp.Net
{
    using System;

    public enum AuthenticationSchemes
    {
        None = 0,
        Digest = 1,
        Basic = 8,
        Anonymous = 0x8000
    }
}

