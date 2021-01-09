namespace SharpCompress.Common
{
    using System;
    using System.IO;

    public abstract class FilePart
    {
        protected FilePart()
        {
        }

        internal abstract Stream GetStream();

        internal abstract string FilePartName { get; }
    }
}

