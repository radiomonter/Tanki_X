namespace Platform.Library.ClientResources.API
{
    using SharpCompress.Common;
    using SharpCompress.Compressor;
    using SharpCompress.Compressor.Deflate;
    using SharpCompress.Reader;
    using SharpCompress.Writer;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;

    public static class TarUtility
    {
        public static void Compress(string fromPath, Stream stream, CompressionType compresType, Func<string, bool> filter = null)
        {
            using (IWriter writer = WriterFactory.Open(stream, ArchiveType.Tar, compresType))
            {
                foreach (string str in FileUtils.GetFiles(fromPath, filter))
                {
                    using (FileStream stream2 = new FileStream(str, FileMode.Open, FileAccess.Read))
                    {
                        writer.Write(str.Substring(fromPath.Length + 1), stream2, new DateTime?(DateTime.Now));
                    }
                }
            }
        }

        public static void Extract(Stream stream, string toPath)
        {
            using (IReader reader = ReaderFactory.Open(new GZipStream(stream, CompressionMode.Decompress), Options.KeepStreamsOpen))
            {
                while (reader.MoveToNextEntry())
                {
                    if (reader.Entry.IsDirectory)
                    {
                        continue;
                    }
                    string path = toPath + "/" + reader.Entry.FilePath;
                    string directoryName = Path.GetDirectoryName(path);
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                    using (FileStream stream3 = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        reader.WriteEntryTo(stream3);
                    }
                }
            }
        }
    }
}

