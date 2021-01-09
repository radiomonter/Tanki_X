namespace SharpCompress.Archive.Tar
{
    using SharpCompress;
    using SharpCompress.Archive;
    using SharpCompress.Common;
    using SharpCompress.Common.Tar;
    using SharpCompress.Common.Tar.Headers;
    using SharpCompress.IO;
    using SharpCompress.Reader;
    using SharpCompress.Reader.Tar;
    using SharpCompress.Writer.Tar;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class TarArchive : AbstractWritableArchive<TarArchiveEntry, TarVolume>
    {
        [CompilerGenerated]
        private static Func<TarArchiveEntry, bool> <>f__am$cache0;

        internal TarArchive() : base(ArchiveType.Tar)
        {
        }

        internal TarArchive(Stream stream, Options options) : base(ArchiveType.Tar, stream.AsEnumerable<Stream>(), options)
        {
        }

        public static TarArchive Create() => 
            new TarArchive();

        protected override TarArchiveEntry CreateEntry(string filePath, Stream source, long size, DateTime? modified, bool closeStream) => 
            new TarWritableArchiveEntry(this, source, CompressionType.Unknown, filePath, size, modified, closeStream);

        protected override IReader CreateReaderForSolidExtraction()
        {
            Stream stream = base.Volumes.Single<TarVolume>().Stream;
            stream.Position = 0L;
            return TarReader.Open(stream, Options.KeepStreamsOpen);
        }

        public static bool IsTarFile(Stream stream)
        {
            bool flag;
            try
            {
                TarHeader header = new TarHeader();
                header.Read(new BinaryReader(stream));
                flag = (header.Name.Length > 0) && Enum.IsDefined(typeof(EntryType), header.EntryType);
            }
            catch
            {
                return false;
            }
            return flag;
        }

        [DebuggerHidden]
        protected override IEnumerable<TarArchiveEntry> LoadEntries(IEnumerable<TarVolume> volumes) => 
            new <LoadEntries>c__Iterator0 { 
                volumes = volumes,
                $this = this,
                $PC = -2
            };

        protected override IEnumerable<TarVolume> LoadVolumes(IEnumerable<Stream> streams, Options options) => 
            new TarVolume(streams.First<Stream>(), options).AsEnumerable<TarVolume>();

        public static TarArchive Open(Stream stream)
        {
            stream.CheckNotNull("stream");
            return Open(stream, Options.None);
        }

        public static TarArchive Open(Stream stream, Options options)
        {
            stream.CheckNotNull("stream");
            return new TarArchive(stream, options);
        }

        protected override void SaveTo(Stream stream, CompressionInfo compressionInfo, IEnumerable<TarArchiveEntry> oldEntries, IEnumerable<TarArchiveEntry> newEntries)
        {
            using (TarWriter writer = new TarWriter(stream, compressionInfo))
            {
                if (<>f__am$cache0 == null)
                {
                    <>f__am$cache0 = x => !x.IsDirectory;
                }
                foreach (TarArchiveEntry entry in oldEntries.Concat<TarArchiveEntry>(newEntries).Where<TarArchiveEntry>(<>f__am$cache0))
                {
                    using (Stream stream2 = entry.OpenEntryStream())
                    {
                        writer.Write(entry.FilePath, stream2, entry.LastModifiedTime, new long?(entry.Size));
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <LoadEntries>c__Iterator0 : IEnumerable, IEnumerable<TarArchiveEntry>, IEnumerator, IDisposable, IEnumerator<TarArchiveEntry>
        {
            internal IEnumerable<TarVolume> volumes;
            internal Stream <stream>__0;
            internal TarHeader <previousHeader>__0;
            internal IEnumerator<TarHeader> $locvar0;
            internal TarHeader <header>__1;
            internal TarArchive $this;
            internal TarArchiveEntry $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                uint num = (uint) this.$PC;
                this.$disposing = true;
                this.$PC = -1;
                switch (num)
                {
                    case 1:
                        try
                        {
                        }
                        finally
                        {
                            if (this.$locvar0 != null)
                            {
                                this.$locvar0.Dispose();
                            }
                        }
                        break;

                    default:
                        break;
                }
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                bool flag = false;
                switch (num)
                {
                    case 0:
                        this.<stream>__0 = this.volumes.Single<TarVolume>().Stream;
                        this.<previousHeader>__0 = null;
                        this.$locvar0 = TarHeaderFactory.ReadHeader(StreamingMode.Seekable, this.<stream>__0).GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                        break;

                    default:
                        goto TR_0000;
                }
                try
                {
                    switch (num)
                    {
                        default:
                            while (true)
                            {
                                if (this.$locvar0.MoveNext())
                                {
                                    this.<header>__1 = this.$locvar0.Current;
                                    if (this.<header>__1 == null)
                                    {
                                        continue;
                                    }
                                    if (this.<header>__1.EntryType == EntryType.LongName)
                                    {
                                        this.<previousHeader>__0 = this.<header>__1;
                                        continue;
                                    }
                                    if (this.<previousHeader>__0 != null)
                                    {
                                        MemoryStream streamToWriteTo = new MemoryStream();
                                        new TarArchiveEntry(this.$this, new TarFilePart(this.<previousHeader>__0, this.<stream>__0), CompressionType.None).WriteTo(streamToWriteTo);
                                        streamToWriteTo.Position = 0L;
                                        byte[] bytes = streamToWriteTo.ToArray();
                                        this.<header>__1.Name = ArchiveEncoding.Default.GetString(bytes, 0, bytes.Length).TrimNulls();
                                        this.<previousHeader>__0 = null;
                                    }
                                    this.$current = new TarArchiveEntry(this.$this, new TarFilePart(this.<header>__1, this.<stream>__0), CompressionType.None);
                                    if (!this.$disposing)
                                    {
                                        this.$PC = 1;
                                    }
                                    flag = true;
                                }
                                else
                                {
                                    this.$PC = -1;
                                    goto TR_0000;
                                }
                                break;
                            }
                            break;
                    }
                }
                finally
                {
                    if (flag)
                    {
                    }
                    if (this.$locvar0 != null)
                    {
                        this.$locvar0.Dispose();
                    }
                }
                return true;
            TR_0000:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<TarArchiveEntry> IEnumerable<TarArchiveEntry>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new TarArchive.<LoadEntries>c__Iterator0 { 
                    $this = this.$this,
                    volumes = this.volumes
                };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<SharpCompress.Archive.Tar.TarArchiveEntry>.GetEnumerator();

            TarArchiveEntry IEnumerator<TarArchiveEntry>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

