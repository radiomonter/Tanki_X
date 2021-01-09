namespace WebSocketSharp.Net
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Text;

    internal class ChunkStream
    {
        private int _chunkRead;
        private int _chunkSize;
        private List<Chunk> _chunks;
        private bool _gotIt;
        private WebHeaderCollection _headers;
        private StringBuilder _saved;
        private bool _sawCr;
        private InputChunkState _state;
        private int _trailerState;

        public ChunkStream(WebHeaderCollection headers)
        {
            this._headers = headers;
            this._chunkSize = -1;
            this._chunks = new List<Chunk>();
            this._saved = new StringBuilder();
        }

        public ChunkStream(byte[] buffer, int offset, int count, WebHeaderCollection headers) : this(headers)
        {
            this.Write(buffer, offset, count);
        }

        private int read(byte[] buffer, int offset, int count)
        {
            int num = 0;
            int num2 = this._chunks.Count;
            for (int i = 0; i < num2; i++)
            {
                Chunk chunk = this._chunks[i];
                if (chunk != null)
                {
                    if (chunk.ReadLeft == 0)
                    {
                        this._chunks[i] = null;
                    }
                    else
                    {
                        num += chunk.Read(buffer, offset + num, count - num);
                        if (num == count)
                        {
                            break;
                        }
                    }
                }
            }
            return num;
        }

        public int Read(byte[] buffer, int offset, int count) => 
            (count > 0) ? this.read(buffer, offset, count) : 0;

        private static string removeChunkExtension(string value)
        {
            int index = value.IndexOf(';');
            return ((index <= -1) ? value : value.Substring(0, index));
        }

        internal void ResetBuffer()
        {
            this._chunkRead = 0;
            this._chunkSize = -1;
            this._chunks.Clear();
        }

        private InputChunkState seekCrLf(byte[] buffer, ref int offset, int length)
        {
            int num;
            if (!this._sawCr)
            {
                offset = (num = offset) + 1;
                if (buffer[num] != 13)
                {
                    throwProtocolViolation("CR is expected.");
                }
                this._sawCr = true;
                if (offset == length)
                {
                    return InputChunkState.DataEnded;
                }
            }
            offset = (num = offset) + 1;
            if (buffer[num] != 10)
            {
                throwProtocolViolation("LF is expected.");
            }
            return InputChunkState.None;
        }

        private InputChunkState setChunkSize(byte[] buffer, ref int offset, int length)
        {
            byte num = 0;
            while (true)
            {
                if (offset < length)
                {
                    int num2;
                    offset = (num2 = offset) + 1;
                    num = buffer[num2];
                    if (!this._sawCr)
                    {
                        if (num == 13)
                        {
                            this._sawCr = true;
                            continue;
                        }
                        if (num == 10)
                        {
                            throwProtocolViolation("LF is unexpected.");
                        }
                        if (num == 0x20)
                        {
                            this._gotIt = true;
                        }
                        if (!this._gotIt)
                        {
                            this._saved.Append((char) num);
                        }
                        if (this._saved.Length > 20)
                        {
                            throwProtocolViolation("The chunk size is too long.");
                        }
                        continue;
                    }
                    if (num != 10)
                    {
                        throwProtocolViolation("LF is expected.");
                    }
                }
                if (!this._sawCr || (num != 10))
                {
                    return InputChunkState.None;
                }
                this._chunkRead = 0;
                try
                {
                    this._chunkSize = int.Parse(removeChunkExtension(this._saved.ToString()), NumberStyles.HexNumber);
                }
                catch
                {
                    throwProtocolViolation("The chunk size cannot be parsed.");
                }
                if (this._chunkSize != 0)
                {
                    return InputChunkState.Data;
                }
                this._trailerState = 2;
                return InputChunkState.Trailer;
            }
        }

        private InputChunkState setTrailer(byte[] buffer, ref int offset, int length)
        {
            string str;
            if ((this._trailerState == 2) && ((buffer[offset] == 13) && (this._saved.Length == 0)))
            {
                offset++;
                if ((offset < length) && (buffer[offset] == 10))
                {
                    offset++;
                    return InputChunkState.End;
                }
                offset--;
            }
            while ((offset < length) && (this._trailerState < 4))
            {
                int num2;
                offset = (num2 = offset) + 1;
                byte num = buffer[num2];
                this._saved.Append((char) num);
                if (this._saved.Length > 0x1064)
                {
                    throwProtocolViolation("The trailer is too long.");
                }
                if ((this._trailerState == 1) || (this._trailerState == 3))
                {
                    if (num != 10)
                    {
                        throwProtocolViolation("LF is expected.");
                    }
                    this._trailerState++;
                }
                else if (num == 13)
                {
                    this._trailerState++;
                }
                else
                {
                    if (num == 10)
                    {
                        throwProtocolViolation("LF is unexpected.");
                    }
                    this._trailerState = 0;
                }
            }
            if (this._trailerState < 4)
            {
                return InputChunkState.Trailer;
            }
            this._saved.Length -= 2;
            StringReader reader = new StringReader(this._saved.ToString());
            while (((str = reader.ReadLine()) != null) && (str.Length > 0))
            {
                this._headers.Add(str);
            }
            return InputChunkState.End;
        }

        private static void throwProtocolViolation(string message)
        {
            throw new WebException(message, null, WebExceptionStatus.ServerProtocolViolation, null);
        }

        private void write(byte[] buffer, ref int offset, int length)
        {
            if (this._state == InputChunkState.End)
            {
                throwProtocolViolation("The chunks were ended.");
            }
            if (this._state == InputChunkState.None)
            {
                this._state = this.setChunkSize(buffer, ref offset, length);
                if (this._state == InputChunkState.None)
                {
                    return;
                }
                this._saved.Length = 0;
                this._sawCr = false;
                this._gotIt = false;
            }
            if ((this._state == InputChunkState.Data) && (offset < length))
            {
                this._state = this.writeData(buffer, ref offset, length);
                if (this._state == InputChunkState.Data)
                {
                    return;
                }
            }
            if ((this._state == InputChunkState.DataEnded) && (offset < length))
            {
                this._state = this.seekCrLf(buffer, ref offset, length);
                if (this._state == InputChunkState.DataEnded)
                {
                    return;
                }
                this._sawCr = false;
            }
            if ((this._state == InputChunkState.Trailer) && (offset < length))
            {
                this._state = this.setTrailer(buffer, ref offset, length);
                if (this._state == InputChunkState.Trailer)
                {
                    return;
                }
                this._saved.Length = 0;
            }
            if (offset < length)
            {
                this.write(buffer, ref offset, length);
            }
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            if (count > 0)
            {
                this.write(buffer, ref offset, offset + count);
            }
        }

        internal int WriteAndReadBack(byte[] buffer, int offset, int writeCount, int readCount)
        {
            this.Write(buffer, offset, writeCount);
            return this.Read(buffer, offset, readCount);
        }

        private InputChunkState writeData(byte[] buffer, ref int offset, int length)
        {
            int count = length - offset;
            int num2 = this._chunkSize - this._chunkRead;
            if (count > num2)
            {
                count = num2;
            }
            byte[] dst = new byte[count];
            Buffer.BlockCopy(buffer, offset, dst, 0, count);
            this._chunks.Add(new Chunk(dst));
            offset += count;
            this._chunkRead += count;
            return ((this._chunkRead != this._chunkSize) ? InputChunkState.Data : InputChunkState.DataEnded);
        }

        internal WebHeaderCollection Headers =>
            this._headers;

        public int ChunkLeft =>
            this._chunkSize - this._chunkRead;

        public bool WantMore =>
            this._state != InputChunkState.End;
    }
}

