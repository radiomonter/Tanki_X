namespace SharpCompress.Common
{
    using SharpCompress.Compressor.Deflate;
    using System;
    using System.Runtime.CompilerServices;

    public class CompressionInfo
    {
        public CompressionInfo()
        {
            this.DeflateCompressionLevel = CompressionLevel.Default;
        }

        public static implicit operator CompressionInfo(CompressionType compressionType) => 
            new CompressionInfo { Type = compressionType };

        public CompressionType Type { get; set; }

        public CompressionLevel DeflateCompressionLevel { get; set; }
    }
}

