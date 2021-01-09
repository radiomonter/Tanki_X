namespace log4net.Util.TypeConverters
{
    using System;

    public interface IConvertTo
    {
        bool CanConvertTo(Type targetType);
        object ConvertTo(object source, Type targetType);
    }
}

