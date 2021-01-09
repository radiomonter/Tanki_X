namespace log4net.Util.TypeConverters
{
    using log4net.Util;
    using System;

    internal class TypeConverter : IConvertFrom
    {
        public bool CanConvertFrom(Type sourceType) => 
            ReferenceEquals(sourceType, typeof(string));

        public object ConvertFrom(object source)
        {
            string typeName = source as string;
            if (typeName == null)
            {
                throw ConversionNotSupportedException.Create(typeof(Type), source);
            }
            return SystemInfo.GetTypeFromString(typeName, true, true);
        }
    }
}

