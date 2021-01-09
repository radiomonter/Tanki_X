namespace SharpCompress.Reader
{
    using SharpCompress;
    using SharpCompress.Common;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public abstract class AbstractReader<TEntry, TVolume> : IReader, IStreamListener, IDisposable where TEntry: SharpCompress.Common.Entry where TVolume: SharpCompress.Common.Volume
    {
        private bool completed;
        private IEnumerator<TEntry> entriesForCurrentReadStream;
        private bool wroteCurrentEntry;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<CompressedBytesReadEventArgs> CompressedBytesRead;
        [CompilerGenerated, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EventHandler<FilePartExtractionBeginEventArgs> FilePartExtractionBegin;

        public event EventHandler<CompressedBytesReadEventArgs> CompressedBytesRead
        {
            add
            {
                EventHandler<CompressedBytesReadEventArgs> compressedBytesRead = this.CompressedBytesRead;
                while (true)
                {
                    EventHandler<CompressedBytesReadEventArgs> objB = compressedBytesRead;
                    compressedBytesRead = Interlocked.CompareExchange<EventHandler<CompressedBytesReadEventArgs>>(ref this.CompressedBytesRead, objB + value, compressedBytesRead);
                    if (ReferenceEquals(compressedBytesRead, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<CompressedBytesReadEventArgs> compressedBytesRead = this.CompressedBytesRead;
                while (true)
                {
                    EventHandler<CompressedBytesReadEventArgs> objB = compressedBytesRead;
                    compressedBytesRead = Interlocked.CompareExchange<EventHandler<CompressedBytesReadEventArgs>>(ref this.CompressedBytesRead, objB - value, compressedBytesRead);
                    if (ReferenceEquals(compressedBytesRead, objB))
                    {
                        return;
                    }
                }
            }
        }

        public event EventHandler<FilePartExtractionBeginEventArgs> FilePartExtractionBegin
        {
            add
            {
                EventHandler<FilePartExtractionBeginEventArgs> filePartExtractionBegin = this.FilePartExtractionBegin;
                while (true)
                {
                    EventHandler<FilePartExtractionBeginEventArgs> objB = filePartExtractionBegin;
                    filePartExtractionBegin = Interlocked.CompareExchange<EventHandler<FilePartExtractionBeginEventArgs>>(ref this.FilePartExtractionBegin, objB + value, filePartExtractionBegin);
                    if (ReferenceEquals(filePartExtractionBegin, objB))
                    {
                        return;
                    }
                }
            }
            remove
            {
                EventHandler<FilePartExtractionBeginEventArgs> filePartExtractionBegin = this.FilePartExtractionBegin;
                while (true)
                {
                    EventHandler<FilePartExtractionBeginEventArgs> objB = filePartExtractionBegin;
                    filePartExtractionBegin = Interlocked.CompareExchange<EventHandler<FilePartExtractionBeginEventArgs>>(ref this.FilePartExtractionBegin, objB - value, filePartExtractionBegin);
                    if (ReferenceEquals(filePartExtractionBegin, objB))
                    {
                        return;
                    }
                }
            }
        }

        internal AbstractReader(SharpCompress.Common.Options options, SharpCompress.Common.ArchiveType archiveType)
        {
            this.ArchiveType = archiveType;
            this.Options = options;
        }

        public void Dispose()
        {
            if (this.entriesForCurrentReadStream != null)
            {
                this.entriesForCurrentReadStream.Dispose();
            }
            this.Volume.Dispose();
        }

        internal abstract IEnumerable<TEntry> GetEntries(Stream stream);
        protected virtual EntryStream GetEntryStream() => 
            new EntryStream(this.Entry.Parts.First<FilePart>().GetStream());

        internal bool LoadStreamForReading(Stream stream)
        {
            if (this.entriesForCurrentReadStream != null)
            {
                this.entriesForCurrentReadStream.Dispose();
            }
            if ((stream == null) || !stream.CanRead)
            {
                throw new MultipartStreamRequiredException("File is split into multiple archives: '" + this.Entry.FilePath + "'. A new readable stream is required.  Use Cancel if it was intended.");
            }
            this.entriesForCurrentReadStream = this.GetEntries(stream).GetEnumerator();
            return this.entriesForCurrentReadStream.MoveNext();
        }

        public bool MoveToNextEntry()
        {
            if (!this.completed)
            {
                if (this.entriesForCurrentReadStream == null)
                {
                    return this.LoadStreamForReading(this.RequestInitialStream());
                }
                if (!this.wroteCurrentEntry)
                {
                    this.SkipEntry();
                }
                this.wroteCurrentEntry = false;
                if (this.NextEntryForCurrentStream())
                {
                    return true;
                }
                this.completed = true;
            }
            return false;
        }

        internal virtual bool NextEntryForCurrentStream() => 
            this.entriesForCurrentReadStream.MoveNext();

        public EntryStream OpenEntryStream()
        {
            if (this.wroteCurrentEntry)
            {
                throw new ArgumentException("WriteEntryTo or OpenEntryStream can only be called once.");
            }
            EntryStream entryStream = this.GetEntryStream();
            this.wroteCurrentEntry = true;
            return entryStream;
        }

        internal virtual Stream RequestInitialStream() => 
            this.Volume.Stream;

        void IStreamListener.FireCompressedBytesRead(long currentPartCompressedBytes, long compressedReadBytes)
        {
            if (this.CompressedBytesRead != null)
            {
                CompressedBytesReadEventArgs e = new CompressedBytesReadEventArgs {
                    CurrentFilePartCompressedBytesRead = currentPartCompressedBytes,
                    CompressedBytesRead = compressedReadBytes
                };
                this.CompressedBytesRead(this, e);
            }
        }

        void IStreamListener.FireFilePartExtractionBegin(string name, long size, long compressedSize)
        {
            if (this.FilePartExtractionBegin != null)
            {
                FilePartExtractionBeginEventArgs e = new FilePartExtractionBeginEventArgs {
                    CompressedSize = compressedSize,
                    Size = size,
                    Name = name
                };
                this.FilePartExtractionBegin(this, e);
            }
        }

        internal void Skip()
        {
            byte[] buffer = new byte[0x1000];
            using (Stream stream = this.OpenEntryStream())
            {
                while (stream.Read(buffer, 0, buffer.Length) > 0)
                {
                }
            }
        }

        private void SkipEntry()
        {
            if (!this.Entry.IsDirectory)
            {
                this.Skip();
            }
        }

        internal void Write(Stream writeStream)
        {
            using (Stream stream = this.OpenEntryStream())
            {
                stream.TransferTo(writeStream);
            }
        }

        public void WriteEntryTo(Stream writableStream)
        {
            if (this.wroteCurrentEntry)
            {
                throw new ArgumentException("WriteEntryTo or OpenEntryStream can only be called once.");
            }
            if ((writableStream == null) || !writableStream.CanWrite)
            {
                throw new ArgumentNullException("A writable Stream was required.  Use Cancel if that was intended.");
            }
            this.Write(writableStream);
            this.wroteCurrentEntry = true;
        }

        IEntry IReader.Entry =>
            this.Entry;

        internal SharpCompress.Common.Options Options { get; private set; }

        public SharpCompress.Common.ArchiveType ArchiveType { get; private set; }

        public abstract TVolume Volume { get; }

        public TEntry Entry =>
            this.entriesForCurrentReadStream.Current;
    }
}

