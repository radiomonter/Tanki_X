namespace log4net.Core
{
    using log4net.Appender;
    using System;

    public interface IAppenderAttachable
    {
        void AddAppender(IAppender appender);
        IAppender GetAppender(string name);
        void RemoveAllAppenders();
        IAppender RemoveAppender(IAppender appender);
        IAppender RemoveAppender(string name);

        AppenderCollection Appenders { get; }
    }
}

