namespace log4net.Util.TypeConverters
{
    using System;

    internal class BooleanConverter : IConvertFrom
    {
        public bool CanConvertFrom(Type sourceType) => 
            ReferenceEquals(sourceType, typeof(string));

        public object ConvertFrom(object source)
        {
            string str = source as string;
            if (str == null)
            {
                throw ConversionNotSupportedException.Create(typeof(bool), source);
            }
            return bool.Parse(str);
        }
    }
}

