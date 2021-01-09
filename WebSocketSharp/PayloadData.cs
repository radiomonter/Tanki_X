namespace WebSocketSharp
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    internal class PayloadData : IEnumerable<byte>, IEnumerable
    {
        private byte[] _data;
        private long _extDataLength;
        private long _length;
        public static readonly PayloadData Empty = new PayloadData();
        public static readonly ulong MaxLength = 0x7fffffffffffffffUL;

        internal PayloadData()
        {
            this._data = WebSocket.EmptyBytes;
        }

        internal PayloadData(byte[] data) : this(data, data.LongLength)
        {
        }

        internal PayloadData(byte[] data, long length)
        {
            this._data = data;
            this._length = length;
        }

        [DebuggerHidden]
        public IEnumerator<byte> GetEnumerator() => 
            new <GetEnumerator>c__Iterator0 { $this = this };

        internal void Mask(byte[] key)
        {
            for (long i = 0L; i < this._length; i += 1L)
            {
                this._data[(int) ((IntPtr) i)] = (byte) (this._data[(int) ((IntPtr) i)] ^ key[(int) ((IntPtr) (i % 4L))]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            this.GetEnumerator();

        public byte[] ToArray() => 
            this._data;

        public override string ToString() => 
            BitConverter.ToString(this._data);

        internal long ExtensionDataLength
        {
            get => 
                this._extDataLength;
            set => 
                this._extDataLength = value;
        }

        internal bool IncludesReservedCloseStatusCode =>
            (this._length > 1L) && this._data.SubArray<byte>(0, 2).ToUInt16(ByteOrder.Big).IsReserved();

        public byte[] ApplicationData =>
            (this._extDataLength <= 0L) ? this._data : this._data.SubArray<byte>(this._extDataLength, (this._length - this._extDataLength));

        public byte[] ExtensionData =>
            (this._extDataLength <= 0L) ? WebSocket.EmptyBytes : this._data.SubArray<byte>(0L, this._extDataLength);

        public ulong Length =>
            (ulong) this._length;

        [CompilerGenerated]
        private sealed class <GetEnumerator>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<byte>
        {
            internal byte[] $locvar0;
            internal int $locvar1;
            internal byte <b>__1;
            internal PayloadData $this;
            internal byte $current;
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
                        this.$locvar0 = this.$this._data;
                        this.$locvar1 = 0;
                        break;

                    case 1:
                        this.$locvar1++;
                        break;

                    default:
                        goto TR_0000;
                }
                if (this.$locvar1 < this.$locvar0.Length)
                {
                    this.<b>__1 = this.$locvar0[this.$locvar1];
                    this.$current = this.<b>__1;
                    if (!this.$disposing)
                    {
                        this.$PC = 1;
                    }
                    return true;
                }
                this.$PC = -1;
            TR_0000:
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            byte IEnumerator<byte>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

