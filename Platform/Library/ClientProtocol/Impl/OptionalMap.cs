namespace Platform.Library.ClientProtocol.Impl
{
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Text;

    public class OptionalMap : IOptionalMap
    {
        private const int SIZE_QUANT = 0x1388;
        private int readPosition;
        private int size;
        private int capacity;
        private byte[] map;

        public OptionalMap()
        {
            this.Fill(new byte[0x1388], 0, 0x9c40);
        }

        public OptionalMap(byte[] map, int size)
        {
            this.Fill(map, size);
        }

        public void Add(bool isNull)
        {
            this.SetBit(this.size, isNull);
            this.size++;
        }

        public void Clear()
        {
            Array.Clear(this.map, 0, this.map.Length);
            this.size = 0;
            this.readPosition = 0;
            this.capacity = 0x9c40;
        }

        public void Concat(IOptionalMap otherMap)
        {
            int size = otherMap.GetSize();
            for (int i = 0; i < size; i++)
            {
                this.Add(((OptionalMap) otherMap).GetBit(i));
            }
        }

        public IOptionalMap Duplicate()
        {
            OptionalMap map = new OptionalMap {
                capacity = this.capacity,
                readPosition = this.readPosition,
                size = this.size
            };
            Array.Copy(this.map, 0, map.map, 0, this.map.Length);
            return map;
        }

        public void Fill(byte[] map, int size)
        {
            this.map = map;
            this.size = size;
            this.readPosition = 0;
            this.capacity = size << 3;
        }

        private void Fill(byte[] map, int size, int capacity)
        {
            this.map = map;
            this.size = size;
            this.readPosition = 0;
            this.capacity = capacity;
        }

        public void Flip()
        {
            this.readPosition = 0;
        }

        public bool Get()
        {
            if (this.readPosition >= this.size)
            {
                throw new IndexOutOfRangeException();
            }
            bool bit = this.GetBit(this.readPosition);
            this.readPosition++;
            return bit;
        }

        private bool GetBit(int position)
        {
            int index = position >> 3;
            return ((this.map[index] & (1 << ((7 ^ (position & 7)) & 0x1f))) != 0);
        }

        public bool GetLast()
        {
            int num;
            this.size = num = this.size - 1;
            return this.GetBit(num);
        }

        public byte[] GetMap() => 
            this.map;

        public int GetReadPosition() => 
            this.readPosition;

        public int GetSize() => 
            this.size;

        public bool Has() => 
            this.GetReadPosition() < this.GetSize();

        public void Reset()
        {
            this.readPosition = 0;
        }

        private void SetBit(int position, bool value)
        {
            int index = position >> 3;
            int num2 = 7 ^ (position & 7);
            this.map[index] = !value ? ((byte) (this.map[index] & ((byte) (0xff ^ (1 << (num2 & 0x1f)))))) : ((byte) (this.map[index] | (1 << (num2 & 0x1f))));
        }

        public void SetSize(int size)
        {
            this.size = size;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("optional map [ size:").Append(this.size).Append(" data:");
            for (int i = 0; i < this.size; i++)
            {
                if (this.GetBit(i))
                {
                    builder.Append("1");
                }
                else
                {
                    builder.Append("0");
                }
                if (i == this.readPosition)
                {
                    builder.Append("<->");
                }
            }
            builder.Append("]");
            return builder.ToString();
        }
    }
}

