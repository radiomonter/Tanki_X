namespace Platform.Library.ClientProtocol.API
{
    using System;
    using System.IO;

    public class StreamDumper
    {
        public static string Dump(Stream data)
        {
            long position = data.Position;
            data.Seek(0L, SeekOrigin.Begin);
            BinaryReader reader = new BinaryReader(data);
            string str = "\n=== Dump data ===\n";
            int num2 = 0;
            string str2 = string.Empty;
            for (long i = 0L; i < data.Length; i += 1L)
            {
                byte num4 = reader.ReadByte();
                char c = (char) num4;
                str = str + num4.ToString("X2") + " ";
                str2 = (char.IsWhiteSpace(c) || char.IsControl(c)) ? (str2 + '.') : (str2 + c);
                if ((num2 + 1) > 0x10)
                {
                    str = str + "\t" + str2 + "\n";
                    num2 = 0;
                    str2 = string.Empty;
                }
            }
            if (num2 != 0)
            {
                while (true)
                {
                    if (num2 >= 0x12)
                    {
                        str = str + "\t" + str2 + "\n";
                        break;
                    }
                    num2++;
                    str = str + "   ";
                }
            }
            data.Seek(position, SeekOrigin.Begin);
            return str;
        }
    }
}

