namespace SharpCompress.Common.Tar
{
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

    internal static class TarHeaderFactory
    {
        private static long PadTo512(long size)
        {
            int num = (int) (size % 0x200L);
            return ((num != 0) ? ((0x200 - num) + size) : size);
        }

        [DebuggerHidden]
        internal static IEnumerable<TarHeader> ReadHeader(StreamingMode mode, Stream stream) => 
            new <ReadHeader>c__Iterator0 { 
                stream = stream,
                mode = mode,
                $PC = -2
            };

        [CompilerGenerated]
        private sealed class <ReadHeader>c__Iterator0 : IEnumerable, IEnumerable<TarHeader>, IEnumerator, IDisposable, IEnumerator<TarHeader>
        {
            internal TarHeader <header>__1;
            internal Stream stream;
            internal StreamingMode mode;
            internal TarHeader $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                    case 1:
                        this.<header>__1 = null;
                        try
                        {
                            BinaryReader reader = new BinaryReader(this.stream);
                            this.<header>__1 = new TarHeader();
                            if (!this.<header>__1.Read(reader))
                            {
                                break;
                            }
                            if (this.mode == StreamingMode.Seekable)
                            {
                                this.<header>__1.DataStartPosition = new long?(reader.BaseStream.Position);
                                Stream baseStream = reader.BaseStream;
                                baseStream.Position += TarHeaderFactory.PadTo512(this.<header>__1.Size);
                            }
                            else
                            {
                                if (this.mode != StreamingMode.Streaming)
                                {
                                    throw new InvalidFormatException("Invalid StreamingMode");
                                }
                                this.<header>__1.PackedStream = new TarReadOnlySubStream(this.stream, this.<header>__1.Size);
                            }
                        }
                        catch
                        {
                            this.<header>__1 = null;
                        }
                        this.$current = this.<header>__1;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    default:
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            [DebuggerHidden]
            IEnumerator<TarHeader> IEnumerable<TarHeader>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new TarHeaderFactory.<ReadHeader>c__Iterator0 { 
                    stream = this.stream,
                    mode = this.mode
                };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<SharpCompress.Common.Tar.Headers.TarHeader>.GetEnumerator();

            TarHeader IEnumerator<TarHeader>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

