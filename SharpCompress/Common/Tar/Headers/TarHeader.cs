namespace SharpCompress.Common.Tar.Headers
{
    using SharpCompress;
    using SharpCompress.Common;
    using System;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;

    internal class TarHeader
    {
        internal static readonly DateTime Epoch = new DateTime(0x7b2, 1, 1, 0, 0, 0);

        internal bool Read(BinaryReader reader)
        {
            byte[] bytes = reader.ReadBytes(0x200);
            if (bytes.Length == 0)
            {
                return false;
            }
            if (bytes.Length < 0x200)
            {
                throw new InvalidOperationException();
            }
            this.Name = ArchiveEncoding.Default.GetString(bytes, 0, 100).TrimNulls();
            this.EntryType = (SharpCompress.Common.Tar.Headers.EntryType) bytes[0x9c];
            if ((bytes[0x7c] & 0x80) != 0x80)
            {
                this.Size = ReadASCIIInt64Base8(bytes, 0x7c, 11);
            }
            else
            {
                long network = BitConverter.ToInt64(bytes, 0x80);
                this.Size = IPAddress.NetworkToHostOrder(network);
            }
            this.LastModifiedTime = Epoch.AddSeconds((double) ReadASCIIInt64Base8(bytes, 0x88, 11));
            this.Magic = ArchiveEncoding.Default.GetString(bytes, 0x101, 6).TrimNulls();
            if (!string.IsNullOrEmpty(this.Magic) && "ustar ".Equals(this.Magic))
            {
                string str = ArchiveEncoding.Default.GetString(bytes, 0x159, 0x9d).TrimNulls();
                if (!string.IsNullOrEmpty(str))
                {
                    this.Name = str + "/" + this.Name;
                }
            }
            return ((this.EntryType == SharpCompress.Common.Tar.Headers.EntryType.LongName) || (this.Name.Length != 0));
        }

        private static int ReadASCIIInt32Base8(byte[] buffer, int offset, int count)
        {
            string str = Encoding.UTF8.GetString(buffer, offset, count).TrimNulls();
            return (!string.IsNullOrEmpty(str) ? Convert.ToInt32(str, 8) : 0);
        }

        private static long ReadASCIIInt64(byte[] buffer, int offset, int count)
        {
            string str = Encoding.UTF8.GetString(buffer, offset, count).TrimNulls();
            return (!string.IsNullOrEmpty(str) ? Convert.ToInt64(str) : 0L);
        }

        private static long ReadASCIIInt64Base8(byte[] buffer, int offset, int count)
        {
            string str = Encoding.UTF8.GetString(buffer, offset, count).TrimNulls();
            return (!string.IsNullOrEmpty(str) ? Convert.ToInt64(str, 8) : 0L);
        }

        internal static int RecalculateAltChecksum(byte[] buf)
        {
            Encoding.UTF8.GetBytes("        ").CopyTo(buf, 0x94);
            int num = 0;
            foreach (byte num2 in buf)
            {
                num = ((num2 & 0x80) != 0x80) ? (num + num2) : (num - (num2 ^ 0x80));
            }
            return num;
        }

        internal static int RecalculateChecksum(byte[] buf)
        {
            Encoding.UTF8.GetBytes("        ").CopyTo(buf, 0x94);
            int num = 0;
            foreach (byte num2 in buf)
            {
                num += num2;
            }
            return num;
        }

        internal unsafe void Write(Stream output)
        {
            if (this.Name.Length > 0xff)
            {
                throw new InvalidFormatException("UsTar fileName can not be longer thatn 255 chars");
            }
            byte[] buffer = new byte[0x200];
            string name = this.Name;
            if (name.Length > 100)
            {
                name = this.Name.Substring(0, 100);
            }
            WriteStringBytes(name, buffer, 0, 100);
            WriteOctalBytes(0x1ffL, buffer, 100, 8);
            WriteOctalBytes(0L, buffer, 0x6c, 8);
            WriteOctalBytes(0L, buffer, 0x74, 8);
            WriteOctalBytes(this.Size, buffer, 0x7c, 12);
            WriteOctalBytes((long) (this.LastModifiedTime - Epoch).TotalSeconds, buffer, 0x88, 12);
            buffer[0x9c] = (byte) this.EntryType;
            if (this.Name.Length > 100)
            {
                name = this.Name.Substring(0x65, this.Name.Length);
                ArchiveEncoding.Default.GetBytes(name).CopyTo(buffer, 0x159);
            }
            if (this.Size >= 0x1ffffffffL)
            {
                byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(this.Size));
                byte[] array = new byte[12];
                bytes.CopyTo(array, (int) (12 - bytes.Length));
                byte* numPtr1 = array;
                numPtr1[0] = (byte) (numPtr1[0] | 0x80);
                array.CopyTo(buffer, 0x7c);
            }
            WriteOctalBytes((long) RecalculateChecksum(buffer), buffer, 0x94, 8);
            output.Write(buffer, 0, buffer.Length);
        }

        private static void WriteOctalBytes(long value, byte[] buffer, int offset, int length)
        {
            string str = Convert.ToString(value, 8);
            int num = (length - str.Length) - 1;
            for (int i = 0; i < num; i++)
            {
                buffer[offset + i] = 0x20;
            }
            for (int j = 0; j < str.Length; j++)
            {
                buffer[(offset + j) + num] = (byte) str[j];
            }
            buffer[offset + length] = 0;
        }

        private static void WriteStringBytes(string name, byte[] buffer, int offset, int length)
        {
            int num = 0;
            while ((num < (length - 1)) && (num < name.Length))
            {
                buffer[offset + num] = (byte) name[num];
                num++;
            }
            while (num < length)
            {
                buffer[offset + num] = 0;
                num++;
            }
        }

        internal string Name { get; set; }

        internal long Size { get; set; }

        internal DateTime LastModifiedTime { get; set; }

        internal SharpCompress.Common.Tar.Headers.EntryType EntryType { get; set; }

        internal Stream PackedStream { get; set; }

        public long? DataStartPosition { get; set; }

        public string Magic { get; set; }
    }
}

