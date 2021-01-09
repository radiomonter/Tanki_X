namespace SharpCompress.Compressor.LZMA.LZ
{
    using SharpCompress.Compressor.LZMA;
    using System;
    using System.IO;

    internal class OutWindow
    {
        private byte[] _buffer;
        private int _windowSize;
        private int _pos;
        private int _streamPos;
        private int _pendingLen;
        private int _pendingDist;
        private Stream _stream;
        public long Total;
        public long Limit;

        public void CopyBlock(int distance, int len)
        {
            int num = len;
            int num2 = (this._pos - distance) - 1;
            if (num2 < 0)
            {
                num2 += this._windowSize;
            }
            while ((num > 0) && ((this._pos < this._windowSize) && (this.Total < this.Limit)))
            {
                int num3;
                if (num2 >= this._windowSize)
                {
                    num2 = 0;
                }
                this._pos = (num3 = this._pos) + 1;
                this._buffer[num3] = this._buffer[num2++];
                this.Total += 1L;
                if (this._pos >= this._windowSize)
                {
                    this.Flush();
                }
                num--;
            }
            this._pendingLen = num;
            this._pendingDist = distance;
        }

        public void CopyPending()
        {
            if (this._pendingLen > 0)
            {
                this.CopyBlock(this._pendingDist, this._pendingLen);
            }
        }

        public int CopyStream(Stream stream, int len)
        {
            int num = len;
            while ((num > 0) && ((this._pos < this._windowSize) && (this.Total < this.Limit)))
            {
                int count = this._windowSize - this._pos;
                if (count > (this.Limit - this.Total))
                {
                    count = (int) (this.Limit - this.Total);
                }
                if (count > num)
                {
                    count = num;
                }
                int num3 = stream.Read(this._buffer, this._pos, count);
                if (num3 == 0)
                {
                    throw new DataErrorException();
                }
                num -= num3;
                this._pos += num3;
                this.Total += num3;
                if (this._pos >= this._windowSize)
                {
                    this.Flush();
                }
            }
            return (len - num);
        }

        public void Create(int windowSize)
        {
            if (this._windowSize != windowSize)
            {
                this._buffer = new byte[windowSize];
            }
            else
            {
                this._buffer[windowSize - 1] = 0;
            }
            this._windowSize = windowSize;
            this._pos = 0;
            this._streamPos = 0;
            this._pendingLen = 0;
            this.Total = 0L;
            this.Limit = 0L;
        }

        public void Flush()
        {
            if (this._stream != null)
            {
                int count = this._pos - this._streamPos;
                if (count != 0)
                {
                    this._stream.Write(this._buffer, this._streamPos, count);
                    if (this._pos >= this._windowSize)
                    {
                        this._pos = 0;
                    }
                    this._streamPos = this._pos;
                }
            }
        }

        public byte GetByte(int distance)
        {
            int index = (this._pos - distance) - 1;
            if (index < 0)
            {
                index += this._windowSize;
            }
            return this._buffer[index];
        }

        public void Init(Stream stream)
        {
            this.ReleaseStream();
            this._stream = stream;
        }

        public void PutByte(byte b)
        {
            int num;
            this._pos = (num = this._pos) + 1;
            this._buffer[num] = b;
            this.Total += 1L;
            if (this._pos >= this._windowSize)
            {
                this.Flush();
            }
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            if (this._streamPos >= this._pos)
            {
                return 0;
            }
            int num = this._pos - this._streamPos;
            if (num > count)
            {
                num = count;
            }
            Buffer.BlockCopy(this._buffer, this._streamPos, buffer, offset, num);
            this._streamPos += num;
            if (this._streamPos >= this._windowSize)
            {
                this._pos = 0;
                this._streamPos = 0;
            }
            return num;
        }

        public void ReleaseStream()
        {
            this.Flush();
            this._stream = null;
        }

        public void Reset()
        {
            this.Create(this._windowSize);
        }

        public void SetLimit(long size)
        {
            this.Limit = this.Total + size;
        }

        public void Train(Stream stream)
        {
            long length = stream.Length;
            int len = (length >= this._windowSize) ? this._windowSize : ((int) length);
            stream.Position = length - len;
            this.Total = 0L;
            this.Limit = len;
            this._pos = this._windowSize - len;
            this.CopyStream(stream, len);
            if (this._pos == this._windowSize)
            {
                this._pos = 0;
            }
            this._streamPos = this._pos;
        }

        public bool HasSpace =>
            (this._pos < this._windowSize) && (this.Total < this.Limit);

        public bool HasPending =>
            this._pendingLen > 0;

        public int AvailableBytes =>
            this._pos - this._streamPos;
    }
}

