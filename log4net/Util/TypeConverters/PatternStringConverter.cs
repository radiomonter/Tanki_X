namespace log4net.Util.TypeConverters
{
    using log4net.Util;
    using System;

    internal class PatternStringConverter : IConvertTo, IConvertFrom
    {
        public bool CanConvertFrom(Type sourceType) => 
            ReferenceEquals(sourceType, typeof(string));

        public bool CanConvertTo(Type targetType) => 
            typeof(string).IsAssignableFrom(targetType);

        public object ConvertFrom(object source)
        {
            string pattern = source as string;
            if (pattern == null)
            {
                throw ConversionNotSupportedException.Create(typeof(PatternString), source);
            }
            return new PatternString(pattern);
        }

        public object ConvertTo(object source, Type targetType)
        {
            PatternString str = source as PatternString;
            if ((str == null) || !this.CanConvertTo(targetType))
            {
                throw ConversionNotSupportedException.Create(targetType, source);
            }
            return str.Format();
        }
    }
}

