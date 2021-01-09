﻿namespace SharpCompress.Common
{
    using System;
    using System.Runtime.CompilerServices;

    public class FilePartExtractionBeginEventArgs : EventArgs
    {
        public string Name { get; internal set; }

        public long Size { get; internal set; }

        public long CompressedSize { get; internal set; }
    }
}

