namespace SharpCompress.Archive
{
    using SharpCompress.Common;
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    public static class IArchiveExtensions
    {
        [CompilerGenerated]
        private static Func<IArchiveEntry, bool> <>f__am$cache0;

        public static void WriteToDirectory(this IArchive archive, string destinationDirectory, ExtractOptions options = 1)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = x => !x.IsDirectory;
            }
            foreach (IArchiveEntry entry in archive.Entries.Where<IArchiveEntry>(<>f__am$cache0))
            {
                entry.WriteToDirectory(destinationDirectory, options);
            }
        }
    }
}

