namespace log4net.Util.TypeConverters
{
    using System;
    using System.Text;

    internal class EncodingConverter : IConvertFrom
    {
        public bool CanConvertFrom(Type sourceType) => 
            ReferenceEquals(sourceType, typeof(string));

        public object ConvertFrom(object source)
        {
            string name = source as string;
            if (name == null)
            {
                throw ConversionNotSupportedException.Create(typeof(Encoding), source);
            }
            return Encoding.GetEncoding(name);
        }
    }
}

