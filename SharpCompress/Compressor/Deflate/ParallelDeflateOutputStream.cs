namespace SharpCompress.Compressor.Deflate
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal class ParallelDeflateOutputStream : Stream
    {
        private static readonly int IO_BUFFER_SIZE_DEFAULT = 0x10000;
        private readonly CompressionLevel _compressLevel;
        private readonly object _eLock;
        private readonly bool _leaveOpen;
        private readonly object _outputLock;
        private readonly ManualResetEvent _sessionReset;
        private readonly ManualResetEvent _writingDone;
        private int _Crc32;
        private TraceBits _DesiredTrace;
        private int _bufferSize;
        private bool _firstWriteDone;
        private bool _isClosed;
        private bool _isDisposed;
        private int _nextToFill;
        private int _nextToWrite;
        private bool _noMoreInputForThisSegment;
        private Stream _outStream;
        private int _pc;
        private volatile Exception _pendingException;
        private List<WorkItem> _pool;
        private long _totalBytesProcessed;

        public ParallelDeflateOutputStream(Stream stream) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, false)
        {
        }

        public ParallelDeflateOutputStream(Stream stream, CompressionLevel level) : this(stream, level, CompressionStrategy.Default, false)
        {
        }

        public ParallelDeflateOutputStream(Stream stream, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
        {
        }

        public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, bool leaveOpen) : this(stream, CompressionLevel.Default, CompressionStrategy.Default, leaveOpen)
        {
        }

        public ParallelDeflateOutputStream(Stream stream, CompressionLevel level, CompressionStrategy strategy, bool leaveOpen)
        {
            this._eLock = new object();
            this._outputLock = new object();
            this._DesiredTrace = TraceBits.WriterThread | TraceBits.Synch | TraceBits.Session | TraceBits.Lifecycle;
            this._bufferSize = IO_BUFFER_SIZE_DEFAULT;
            this._compressLevel = level;
            this._leaveOpen = leaveOpen;
            this.Strategy = strategy;
            this.BuffersPerCore = 4;
            this._writingDone = new ManualResetEvent(false);
            this._sessionReset = new ManualResetEvent(false);
            this._outStream = stream;
        }

        private void _DeflateOne(object wi)
        {
            WorkItem workitem = (WorkItem) wi;
            try
            {
                lock (workitem)
                {
                    if (workitem.status != 2)
                    {
                        throw new InvalidOperationException();
                    }
                    CRC32 crc = new CRC32();
                    crc.SlurpBlock(workitem.buffer, 0, workitem.inputBytesAvailable);
                    this.DeflateOneSegment(workitem);
                    workitem.status = 4;
                    workitem.crc = crc.Crc32Result;
                    Monitor.Pulse(workitem);
                }
            }
            catch (Exception exception)
            {
                lock (this._eLock)
                {
                    if (this._pendingException != null)
                    {
                        this._pendingException = exception;
                    }
                }
            }
        }

        private void _Flush(bool lastInput)
        {
            if (this._isClosed)
            {
                throw new NotSupportedException();
            }
            WorkItem state = this._pool[this._nextToFill % this._pc];
            lock (state)
            {
                if (state.status != 1)
                {
                    if (lastInput)
                    {
                        this._noMoreInputForThisSegment = true;
                    }
                }
                else
                {
                    state.status = 2;
                    this._nextToFill++;
                    if (lastInput)
                    {
                        this._noMoreInputForThisSegment = true;
                    }
                    if (!ThreadPool.QueueUserWorkItem(new WaitCallback(this._DeflateOne), state))
                    {
                        throw new Exception("Cannot enqueue workitem");
                    }
                }
            }
        }

        private void _InitializePoolOfWorkItems()
        {
            this._pool = new List<WorkItem>();
            for (int i = 0; i < (this.BuffersPerCore * Environment.ProcessorCount); i++)
            {
                this._pool.Add(new WorkItem(this._bufferSize, this._compressLevel, this.Strategy));
            }
            this._pc = this._pool.Count;
            for (int j = 0; j < this._pc; j++)
            {
                this._pool[j].index = j;
            }
            this._nextToFill = this._nextToWrite = 0;
        }

        private void _KickoffWriter()
        {
            if (!ThreadPool.QueueUserWorkItem(new WaitCallback(this._PerpetualWriterMethod)))
            {
                throw new Exception("Cannot enqueue writer thread.");
            }
        }

        private void _PerpetualWriterMethod(object state)
        {
            try
            {
                WorkItem item;
                CRC32 crc;
            TR_0020:
                while (true)
                {
                    this._sessionReset.WaitOne();
                    if (this._isDisposed)
                    {
                        break;
                    }
                    this._sessionReset.Reset();
                    item = null;
                    crc = new CRC32();
                    goto TR_001C;
                }
                return;
            TR_000E:
                if (this._noMoreInputForThisSegment)
                {
                }
                if (!this._noMoreInputForThisSegment || (this._nextToWrite != this._nextToFill))
                {
                    goto TR_001C;
                }
                else
                {
                    byte[] buffer = new byte[0x80];
                    ZlibCodec codec = new ZlibCodec();
                    int num2 = codec.InitializeDeflate(this._compressLevel, false);
                    codec.InputBuffer = null;
                    codec.NextIn = 0;
                    codec.AvailableBytesIn = 0;
                    codec.OutputBuffer = buffer;
                    codec.NextOut = 0;
                    codec.AvailableBytesOut = buffer.Length;
                    num2 = codec.Deflate(FlushType.Finish);
                    if ((num2 != 1) && (num2 != 0))
                    {
                        throw new Exception("deflating: " + codec.Message);
                    }
                    if ((buffer.Length - codec.AvailableBytesOut) > 0)
                    {
                        this._outStream.Write(buffer, 0, buffer.Length - codec.AvailableBytesOut);
                    }
                    codec.EndDeflate();
                    this._Crc32 = crc.Crc32Result;
                    this._writingDone.Set();
                }
                goto TR_0020;
            TR_001C:
                while (true)
                {
                    item = this._pool[this._nextToWrite % this._pc];
                    object obj2 = item;
                    lock (obj2)
                    {
                        if (this._noMoreInputForThisSegment)
                        {
                        }
                        while (true)
                        {
                            if (item.status != 4)
                            {
                                int num = 0;
                                while (true)
                                {
                                    if ((item.status != 4) && (!this._noMoreInputForThisSegment || (this._nextToWrite != this._nextToFill)))
                                    {
                                        num++;
                                        Monitor.Pulse(item);
                                        Monitor.Wait(item);
                                        if (item.status == 4)
                                        {
                                        }
                                        continue;
                                    }
                                    if (!this._noMoreInputForThisSegment || (this._nextToWrite != this._nextToFill))
                                    {
                                        break;
                                    }
                                    break;
                                }
                                continue;
                            }
                            else
                            {
                                item.status = 5;
                                this._outStream.Write(item.compressed, 0, item.compressedBytesAvailable);
                                crc.Combine(item.crc, item.inputBytesAvailable);
                                this._totalBytesProcessed += item.inputBytesAvailable;
                                this._nextToWrite++;
                                item.inputBytesAvailable = 0;
                                item.status = 6;
                                Monitor.Pulse(item);
                            }
                            break;
                        }
                    }
                    break;
                }
                goto TR_000E;
            }
            catch (Exception exception)
            {
                lock (this._eLock)
                {
                    if (this._pendingException != null)
                    {
                        this._pendingException = exception;
                    }
                }
            }
        }

        private bool DeflateOneSegment(WorkItem workitem)
        {
            ZlibCodec compressor = workitem.compressor;
            compressor.ResetDeflate();
            compressor.NextIn = 0;
            compressor.AvailableBytesIn = workitem.inputBytesAvailable;
            compressor.NextOut = 0;
            compressor.AvailableBytesOut = workitem.compressed.Length;
            while (true)
            {
                compressor.Deflate(FlushType.None);
                if ((compressor.AvailableBytesIn <= 0) && (compressor.AvailableBytesOut != 0))
                {
                    compressor.Deflate(FlushType.Sync);
                    workitem.compressedBytesAvailable = (int) compressor.TotalBytesOut;
                    return true;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                this._isDisposed = true;
                this._pool = null;
                this._sessionReset.Set();
                this._writingDone.Close();
                this._sessionReset.Close();
                if (!this._isClosed)
                {
                    this._Flush(true);
                    WorkItem item = this._pool[this._nextToFill % this._pc];
                    lock (item)
                    {
                        Monitor.PulseAll(item);
                    }
                    this._writingDone.WaitOne();
                    if (!this._leaveOpen)
                    {
                        this._outStream.Close();
                    }
                    this._isClosed = true;
                }
            }
        }

        public override void Flush()
        {
            this._Flush(false);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public void Reset(Stream stream)
        {
            if (this._firstWriteDone)
            {
                if (this._noMoreInputForThisSegment)
                {
                    this._writingDone.WaitOne();
                    foreach (WorkItem item in this._pool)
                    {
                        item.status = 0;
                    }
                    this._noMoreInputForThisSegment = false;
                    this._nextToFill = this._nextToWrite = 0;
                    this._totalBytesProcessed = 0L;
                    this._Crc32 = 0;
                    this._isClosed = false;
                    this._writingDone.Reset();
                }
                this._outStream = stream;
                this._sessionReset.Set();
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        [Conditional("Trace")]
        private void TraceOutput(TraceBits bits, string format, params object[] varParams)
        {
            if ((bits & this._DesiredTrace) != TraceBits.None)
            {
                lock (this._outputLock)
                {
                    int hashCode = Thread.CurrentThread.GetHashCode();
                    Console.Write("{0:000} PDOS ", hashCode);
                    Console.WriteLine(format, varParams);
                }
            }
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this._isClosed)
            {
                throw new NotSupportedException();
            }
            if (this._pendingException != null)
            {
                throw this._pendingException;
            }
            if (count != 0)
            {
                if (!this._firstWriteDone)
                {
                    this._InitializePoolOfWorkItems();
                    this._KickoffWriter();
                    this._sessionReset.Set();
                    this._firstWriteDone = true;
                }
                while (true)
                {
                    int num = this._nextToFill % this._pc;
                    WorkItem item = this._pool[num];
                    object obj2 = item;
                    lock (obj2)
                    {
                        if ((item.status != 0) && ((item.status != 6) && (item.status != 1)))
                        {
                            int num3 = 0;
                            while (((item.status != 0) && (item.status != 6)) && (item.status != 1))
                            {
                                num3++;
                                Monitor.Pulse(item);
                                Monitor.Wait(item);
                                if ((item.status != 0) && ((item.status == 6) || (item.status == 1)))
                                {
                                }
                            }
                        }
                        else
                        {
                            item.status = 1;
                            int length = ((item.buffer.Length - item.inputBytesAvailable) <= count) ? (item.buffer.Length - item.inputBytesAvailable) : count;
                            Array.Copy(buffer, offset, item.buffer, item.inputBytesAvailable, length);
                            count -= length;
                            offset += length;
                            item.inputBytesAvailable += length;
                            if (item.inputBytesAvailable == item.buffer.Length)
                            {
                                item.status = 2;
                                this._nextToFill++;
                                if (!ThreadPool.QueueUserWorkItem(new WaitCallback(this._DeflateOne), item))
                                {
                                    throw new Exception("Cannot enqueue workitem");
                                }
                            }
                        }
                    }
                    if (count <= 0)
                    {
                        return;
                    }
                }
            }
        }

        public CompressionStrategy Strategy { get; private set; }

        public int BuffersPerCore { get; set; }

        public int BufferSize
        {
            get => 
                this._bufferSize;
            set
            {
                if (value < 0x400)
                {
                    throw new ArgumentException();
                }
                this._bufferSize = value;
            }
        }

        public int Crc32 =>
            this._Crc32;

        public long BytesProcessed =>
            this._totalBytesProcessed;

        public override bool CanSeek =>
            false;

        public override bool CanRead =>
            false;

        public override bool CanWrite =>
            this._outStream.CanWrite;

        public override long Length
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        [Flags]
        private enum TraceBits
        {
            None = 0,
            Write = 1,
            WriteBegin = 2,
            WriteDone = 4,
            WriteWait = 8,
            Flush = 0x10,
            Compress = 0x20,
            Fill = 0x40,
            Lifecycle = 0x80,
            Session = 0x100,
            Synch = 0x200,
            WriterThread = 0x400
        }
    }
}

