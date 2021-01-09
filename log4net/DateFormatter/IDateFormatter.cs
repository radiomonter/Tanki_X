namespace log4net.DateFormatter
{
    using System;
    using System.IO;

    public interface IDateFormatter
    {
        void FormatDate(DateTime dateToFormat, TextWriter writer);
    }
}

