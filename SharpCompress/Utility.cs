namespace SharpCompress
{
    using SharpCompress.Archive;
    using SharpCompress.Common;
    using SharpCompress.IO;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal static class Utility
    {
        public static void AddRange<T>(this ICollection<T> destination, IEnumerable<T> source)
        {
            foreach (T local in source)
            {
                destination.Add(local);
            }
        }

        [DebuggerHidden]
        public static IEnumerable<T> AsEnumerable<T>(this T item) => 
            new <AsEnumerable>c__Iterator0<T> { 
                item = item,
                $PC = -2
            };

        public static bool BinaryEquals(this byte[] source, byte[] target)
        {
            if (source.Length != target.Length)
            {
                return false;
            }
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] != target[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static void CheckNotNull(this object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public static void CheckNotNullOrEmpty(this string obj, string name)
        {
            obj.CheckNotNull(name);
            if (obj.Length == 0)
            {
                throw new ArgumentException("String is empty.");
            }
        }

        public static uint DateTimeToDosTime(this DateTime? dateTime) => 
            (dateTime != null) ? ((uint) ((((((dateTime.Value.Second / 2) | (dateTime.Value.Minute << 5)) | (dateTime.Value.Hour << 11)) | (dateTime.Value.Day << 0x10)) | (dateTime.Value.Month << 0x15)) | ((dateTime.Value.Year - 0x7bc) << 0x19))) : 0;

        public static DateTime DosDateToDateTime(int iTime) => 
            DosDateToDateTime((uint) iTime);

        public static DateTime DosDateToDateTime(uint iTime) => 
            DosDateToDateTime((ushort) (iTime / 0x10000), (ushort) (iTime % 0x10000));

        public static DateTime DosDateToDateTime(ushort iDate, ushort iTime)
        {
            int year = (iDate / 0x200) + 0x7bc;
            int month = (iDate % 0x200) / 0x20;
            int day = (iDate % 0x200) % 0x20;
            int hour = iTime / 0x800;
            int minute = (iTime % 0x800) / 0x20;
            int second = ((iTime % 0x800) % 0x20) * 2;
            if ((iDate == 0xffff) || ((month == 0) || (day == 0)))
            {
                year = 0x7bc;
                month = 1;
                day = 1;
            }
            if (iTime == 0xffff)
            {
                hour = minute = second = 0;
            }
            try
            {
                return new DateTime(year, month, day, hour, minute, second);
            }
            catch
            {
                return new DateTime();
            }
        }

        internal static void Extract<TEntry, TVolume>(this TEntry entry, AbstractArchive<TEntry, TVolume> archive, Stream streamToWriteTo) where TEntry: IArchiveEntry where TVolume: IVolume
        {
            if (entry.IsDirectory)
            {
                throw new ExtractionException("Entry is a file directory and cannot be extracted.");
            }
            archive.EnsureEntriesLoaded();
            archive.FireEntryExtractionBegin(entry);
            archive.FireFilePartExtractionBegin(entry.FilePath, entry.Size, entry.CompressedSize);
            using (Stream stream = new ListeningStream(archive, entry.OpenEntryStream()))
            {
                stream.TransferTo(streamToWriteTo);
            }
            archive.FireEntryExtractionEnd(entry);
        }

        public static void Fill<T>(T[] array, T val) where T: struct
        {
            Fill<T>(array, 0, array.Length, val);
        }

        public static void Fill<T>(T[] array, int fromindex, int toindex, T val) where T: struct
        {
            if (array.Length == 0)
            {
                throw new NullReferenceException();
            }
            if (fromindex > toindex)
            {
                throw new ArgumentException();
            }
            if ((fromindex < 0) || (array.Length < toindex))
            {
                throw new IndexOutOfRangeException();
            }
            for (int i = (fromindex <= 0) ? fromindex : fromindex--; i < toindex; i++)
            {
                array[i] = val;
            }
        }

        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (T local in items)
            {
                action(local);
            }
        }

        public static void incShortLittleEndian(byte[] array, int pos, short incrementValue)
        {
            short num = (short) (BitConverter.ToInt16(array, pos) + incrementValue);
            WriteLittleEndian(array, pos, num);
        }

        public static void Initialize<T>(this T[] array, Func<T> func)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = func();
            }
        }

        public static bool ReadFully(this Stream stream, byte[] buffer)
        {
            int num2;
            int offset = 0;
            while ((num2 = stream.Read(buffer, offset, buffer.Length - offset)) > 0)
            {
                offset += num2;
                if (offset >= buffer.Length)
                {
                    return true;
                }
            }
            return (offset >= buffer.Length);
        }

        public static int readIntBigEndian(byte[] array, int pos) => 
            ((((((0 | (array[pos] & 0xff)) << 8) | (array[pos + 1] & 0xff)) << 8) | (array[pos + 2] & 0xff)) << 8) | (array[pos + 3] & 0xff);

        public static int readIntLittleEndian(byte[] array, int pos) => 
            BitConverter.ToInt32(array, pos);

        public static short readShortLittleEndian(byte[] array, int pos) => 
            BitConverter.ToInt16(array, pos);

        public static void SetSize(this List<byte> list, int count)
        {
            if (count <= list.Count)
            {
                byte[] array = new byte[count];
                list.CopyTo(array, 0);
                list.Clear();
                list.AddRange(array);
            }
            else
            {
                for (int i = list.Count; i < count; i++)
                {
                    list.Add(0);
                }
            }
        }

        public static void Skip(this Stream source, long advanceAmount)
        {
            byte[] buffer = new byte[0x8000];
            int num = 0;
            int count = 0;
            do
            {
                count = buffer.Length;
                if (count > advanceAmount)
                {
                    count = (int) advanceAmount;
                }
                num = source.Read(buffer, 0, count);
                if (num < 0)
                {
                    break;
                }
                advanceAmount -= num;
            }
            while (advanceAmount != 0L);
        }

        public static void SkipAll(this Stream source)
        {
            byte[] buffer = new byte[0x8000];
            while (source.Read(buffer, 0, buffer.Length) == buffer.Length)
            {
            }
        }

        public static ReadOnlyCollection<T> ToReadOnly<T>(this IEnumerable<T> items) => 
            new ReadOnlyCollection<T>(items.ToList<T>());

        public static long TransferTo(this Stream source, Stream destination)
        {
            int num;
            byte[] buffer = new byte[0x1000];
            long num2 = 0L;
            while ((num = source.Read(buffer, 0, buffer.Length)) != 0)
            {
                num2 += num;
                destination.Write(buffer, 0, num);
            }
            return num2;
        }

        public static string TrimNulls(this string source) => 
            source.Replace('\0', ' ').Trim();

        public static byte[] UInt32ToBigEndianBytes(uint x) => 
            new byte[] { (byte) ((x >> 0x18) & 0xff), (byte) ((x >> 0x10) & 0xff), (byte) ((x >> 8) & 0xff), (byte) (x & 0xff) };

        public static int URShift(int number, int bits) => 
            (number < 0) ? ((number >> (bits & 0x1f)) + (2 << (~bits & 0x1f))) : (number >> (bits & 0x1f));

        public static int URShift(int number, long bits) => 
            URShift(number, (int) bits);

        public static long URShift(long number, int bits) => 
            (number < 0L) ? ((number >> (bits & 0x3f)) + (2L << (~bits & 0x3f))) : (number >> (bits & 0x3f));

        public static long URShift(long number, long bits) => 
            URShift(number, (int) bits);

        public static void writeIntBigEndian(byte[] array, int pos, int value)
        {
            array[pos] = (byte) (URShift(value, 0x18) & 0xff);
            array[pos + 1] = (byte) (URShift(value, 0x10) & 0xff);
            array[pos + 2] = (byte) (URShift(value, 8) & 0xff);
            array[pos + 3] = (byte) (value & 0xff);
        }

        public static void WriteLittleEndian(byte[] array, int pos, short value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Copy(bytes, 0, array, pos, bytes.Length);
        }

        public static void WriteLittleEndian(byte[] array, int pos, int value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            Array.Copy(bytes, 0, array, pos, bytes.Length);
        }

        [CompilerGenerated]
        private sealed class <AsEnumerable>c__Iterator0<T> : IEnumerable, IEnumerable<T>, IEnumerator, IDisposable, IEnumerator<T>
        {
            internal T item;
            internal T $current;
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
                        this.$current = this.item;
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                        this.$PC = -1;
                        break;

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
            IEnumerator<T> IEnumerable<T>.GetEnumerator()
            {
                if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
                {
                    return this;
                }
                return new Utility.<AsEnumerable>c__Iterator0<T> { item = this.item };
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator() => 
                this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();

            T IEnumerator<T>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

