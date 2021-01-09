namespace SharpCompress.Compressor.Deflate
{
    using System;

    internal enum ZlibStreamFlavor
    {
        ZLIB = 0x79e,
        DEFLATE = 0x79f,
        GZIP = 0x7a0
    }
}

