namespace SharpCompress.Compressor.Deflate
{
    using System;
    using System.IO;

    internal class CRC32
    {
        private const int BUFFER_SIZE = 0x2000;
        private static readonly uint[] crc32Table;
        private uint runningCrc32Result = uint.MaxValue;
        private long totalBytesRead;

        static CRC32()
        {
            uint num = 0xedb88320;
            crc32Table = new uint[0x100];
            uint index = 0;
            while (index < 0x100)
            {
                uint num4 = index;
                uint num3 = 8;
                while (true)
                {
                    if (num3 <= 0)
                    {
                        crc32Table[index] = num4;
                        index++;
                        break;
                    }
                    num4 = ((num4 & 1) != 1) ? (num4 >> 1) : ((num4 >> 1) ^ num);
                    num3--;
                }
            }
        }

        internal int _InternalComputeCrc32(uint W, byte B) => 
            (int) (crc32Table[(int) ((IntPtr) ((W ^ B) & 0xff))] ^ (W >> 8));

        public void Combine(int crc, int length)
        {
            uint[] square = new uint[0x20];
            uint[] mat = new uint[0x20];
            if (length != 0)
            {
                uint vec = ~this.runningCrc32Result;
                uint num2 = (uint) crc;
                mat[0] = 0xedb88320;
                uint num3 = 1;
                for (int i = 1; i < 0x20; i++)
                {
                    mat[i] = num3;
                    num3 = num3 << 1;
                }
                this.gf2_matrix_square(square, mat);
                this.gf2_matrix_square(mat, square);
                uint num5 = (uint) length;
                do
                {
                    this.gf2_matrix_square(square, mat);
                    if ((num5 & 1) == 1)
                    {
                        vec = this.gf2_matrix_times(square, vec);
                    }
                    num5 = num5 >> 1;
                    if (num5 == 0)
                    {
                        break;
                    }
                    this.gf2_matrix_square(mat, square);
                    if ((num5 & 1) == 1)
                    {
                        vec = this.gf2_matrix_times(mat, vec);
                    }
                    num5 = num5 >> 1;
                }
                while (num5 != 0);
                vec ^= num2;
                this.runningCrc32Result = ~vec;
            }
        }

        public int ComputeCrc32(int W, byte B) => 
            this._InternalComputeCrc32((uint) W, B);

        public int GetCrc32(Stream input) => 
            this.GetCrc32AndCopy(input, null);

        public int GetCrc32AndCopy(Stream input, Stream output)
        {
            if (input == null)
            {
                throw new ZlibException("The input stream must not be null.");
            }
            byte[] buffer = new byte[0x2000];
            int count = 0x2000;
            this.totalBytesRead = 0L;
            int num2 = input.Read(buffer, 0, count);
            if (output != null)
            {
                output.Write(buffer, 0, num2);
            }
            this.totalBytesRead += num2;
            while (num2 > 0)
            {
                this.SlurpBlock(buffer, 0, num2);
                num2 = input.Read(buffer, 0, count);
                if (output != null)
                {
                    output.Write(buffer, 0, num2);
                }
                this.totalBytesRead += num2;
            }
            return (int) ~this.runningCrc32Result;
        }

        private void gf2_matrix_square(uint[] square, uint[] mat)
        {
            for (int i = 0; i < 0x20; i++)
            {
                square[i] = this.gf2_matrix_times(mat, mat[i]);
            }
        }

        private uint gf2_matrix_times(uint[] matrix, uint vec)
        {
            uint num = 0;
            for (int i = 0; vec != 0; i++)
            {
                if ((vec & 1) == 1)
                {
                    num ^= matrix[i];
                }
                vec = vec >> 1;
            }
            return num;
        }

        public void SlurpBlock(byte[] block, int offset, int count)
        {
            if (block == null)
            {
                throw new ZlibException("The data buffer must not be null.");
            }
            for (int i = 0; i < count; i++)
            {
                int index = offset + i;
                this.runningCrc32Result = (this.runningCrc32Result >> 8) ^ crc32Table[(int) ((IntPtr) (block[index] ^ (this.runningCrc32Result & 0xff)))];
            }
            this.totalBytesRead += count;
        }

        public long TotalBytesRead =>
            this.totalBytesRead;

        public int Crc32Result =>
            (int) ~this.runningCrc32Result;
    }
}

