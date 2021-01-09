namespace WebSocketSharp.Net
{
    using System;

    internal class Chunk
    {
        private byte[] _data;
        private int _offset;

        public Chunk(byte[] data)
        {
            this._data = data;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            int num = this._data.Length - this._offset;
            if (num == 0)
            {
                return num;
            }
            if (count > num)
            {
                count = num;
            }
            Buffer.BlockCopy(this._data, this._offset, buffer, offset, count);
            this._offset += count;
            return count;
        }

        public int ReadLeft =>
            this._data.Length - this._offset;
    }
}

