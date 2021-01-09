namespace log4net.Util.TypeConverters
{
    using log4net.Layout;
    using System;

    internal class PatternLayoutConverter : IConvertFrom
    {
        public bool CanConvertFrom(Type sourceType) => 
            ReferenceEquals(sourceType, typeof(string));

        public object ConvertFrom(object source)
        {
            string pattern = source as string;
            if (pattern == null)
            {
                throw ConversionNotSupportedException.Create(typeof(PatternLayout), source);
            }
            return new PatternLayout(pattern);
        }
    }
}

