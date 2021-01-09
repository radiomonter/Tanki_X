namespace SharpCompress.Common
{
    using System;
    using System.IO;

    public class GenericVolume : Volume
    {
        public GenericVolume(Stream stream, Options options) : base(stream, options)
        {
        }

        public override bool IsFirstVolume =>
            true;

        public override bool IsMultiVolume =>
            true;
    }
}

