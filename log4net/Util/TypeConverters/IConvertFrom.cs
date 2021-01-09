namespace log4net.Util.TypeConverters
{
    using System;

    public interface IConvertFrom
    {
        bool CanConvertFrom(Type sourceType);
        object ConvertFrom(object source);
    }
}

