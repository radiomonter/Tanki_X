namespace SharpCompress.Archive
{
    using SharpCompress.Common;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class IArchiveEntryExtensions
    {
        public static void WriteToDirectory(this IArchiveEntry entry, string destinationDirectory, ExtractOptions options = 1)
        {
            string destinationFileName = string.Empty;
            string fileName = Path.GetFileName(entry.FilePath);
            if (!options.HasFlag(ExtractOptions.ExtractFullPath))
            {
                destinationFileName = Path.Combine(destinationDirectory, fileName);
            }
            else
            {
                string directoryName = Path.GetDirectoryName(entry.FilePath);
                string path = Path.Combine(destinationDirectory, directoryName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                destinationFileName = Path.Combine(path, fileName);
            }
            entry.WriteToFile(destinationFileName, options);
        }

        public static void WriteToFile(this IArchiveEntry entry, string destinationFileName, ExtractOptions options = 1)
        {
            if (!entry.IsDirectory)
            {
                FileMode create = FileMode.Create;
                if (!options.HasFlag(ExtractOptions.Overwrite))
                {
                    create = FileMode.CreateNew;
                }
                using (FileStream stream = File.Open(destinationFileName, create))
                {
                    entry.WriteTo(stream);
                }
            }
        }
    }
}

