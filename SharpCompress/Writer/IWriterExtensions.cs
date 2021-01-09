namespace SharpCompress.Writer
{
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    public static class IWriterExtensions
    {
        public static void Write(this IWriter writer, string entryPath, Stream source)
        {
            DateTime? modificationTime = null;
            writer.Write(entryPath, source, modificationTime);
        }
    }
}

