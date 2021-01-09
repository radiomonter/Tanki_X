namespace SharpCompress.Common.Tar
{
    using SharpCompress;
    using SharpCompress.Common;
    using SharpCompress.Common.Tar.Headers;
    using SharpCompress.IO;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class TarEntry : Entry
    {
        private readonly TarFilePart filePart;
        private readonly SharpCompress.Common.CompressionType type;

        internal TarEntry(TarFilePart filePart, SharpCompress.Common.CompressionType type)
        {
            this.filePart = filePart;
            this.type = type;
        }

        internal override void Close()
        {
        }

        [DebuggerHidden]
        internal static IEnumerable<TarEntry> GetEntries(StreamingMode mode, Stream stream, SharpCompress.Common.CompressionType compressionType) => 
            new <GetEntries>c__Iterator0 { 
                mode = mode,
                stream = stream,
                compressionType = compressionType,
                $PC = -2
            };

        public override SharpCompress.Common.CompressionType CompressionType =>
            this.type;

        public override uint Crc =>
            0;

        public override string FilePath =>
            this.filePart.Header.Name;

        public override long CompressedSize =>
            this.filePart.Header.Size;

        public override long Size =>
            this.filePart.Header.Size;

        public override DateTime? LastModifiedTime =>
            new DateTime?(this.filePart.Header.LastModifiedTime);

        public override DateTime? CreatedTime =>
            null;

        public override DateTime? LastAccessedTime =>
            null;

        public override DateTime? ArchivedTime =>
            null;

        public override bool IsEncrypted =>
            false;

        public override bool IsDirectory =>
            this.filePart.Header.EntryType == EntryType.Directory;

        public override bool IsSplit =>
            false;

        internal override IEnumerable<FilePart> Parts =>
            this.filePart.AsEnumerable<FilePart>();

        [CompilerGenerated]
        private sealed class <GetEntries>c__Iterator0 : IEnumerable, IEnumerable<TarEntry>, IEnumerator, IDisposable, IEnumerator<TarEntry>
        {
            internal StreamingMode mode;
            internal Stream stream;
            internal IEnumerator<TarHeader> $locvar0;
            internal TarHeader <h>__1;
            internal CompressionType compressionType;
            internal TarEntry $current;
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
                    case 2:
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
                        this.$locvar0 = TarHeaderFactory.ReadHeader(this.mode, this.stream).GetEnumerator();
                        num = 0xfffffffd;
                        break;

                    case 1:
                    case 2:
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
                                    this.<h>__1 = this.$locvar0.Current;
                                    if (this.<h>__1 == null)
                                    {
                                        continue;
                                    }
                                    if (this.mode == StreamingMode.Seekable)
                                    {
                                        this.$current = new TarEntry(new TarFilePart(this.<h>__1, this.stream), this.compressionType);
                                        if (!this.$disposing)
                                        {
                                            this.$PC = 1;
                                        }
                                        flag = true;
                                    }
                                    else
                                    {
                                        this.$current = new TarEntry(new TarFilePart(this.<h>__1, null), this.compressionType);
                                        if (!this.$disposing)
                                        {
                                            this.$PC = 2;
                                        }
                                        flag = true;
                                    }
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
            IEnumerator<TarEntry> IEnumerable<TarEntry>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new TarEntry.<GetEntries>c__Iterator0 { 
                    mode = this.mode,
                    stream = this.stream,
                    compressionType = this.compressionType
                };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<SharpCompress.Common.Tar.TarEntry>.GetEnumerator();

            TarEntry IEnumerator<TarEntry>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

