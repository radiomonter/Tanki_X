namespace log4net.Repository
{
    using log4net.Appender;
    using System;

    public interface IBasicRepositoryConfigurator
    {
        void Configure(IAppender appender);
        void Configure(params IAppender[] appenders);
    }
}

