namespace log4net.Layout
{
    using log4net.Core;
    using log4net.Util.TypeConverters;
    using System;

    [TypeConverter(typeof(RawLayoutConverter))]
    public interface IRawLayout
    {
        object Format(LoggingEvent loggingEvent);
    }
}

