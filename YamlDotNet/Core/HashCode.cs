﻿namespace YamlDotNet.Core
{
    using System;

    internal static class HashCode
    {
        public static int CombineHashCodes(int h1, int h2) => 
            ((h1 << 5) + h1) ^ h2;
    }
}

