namespace Platform.Library.ClientLogger.API
{
    using log4net;
    using log4net.Appender;
    using log4net.Config;
    using log4net.Core;
    using log4net.Repository.Hierarchy;
    using Platform.Library.ClientLogger.Impl;
    using System;
    using System.Collections;
    using System.IO;

    public static class LoggerProvider
    {
        public static void AddApender(AppenderSkeleton appender)
        {
            BasicConfigurator.Configure(appender);
        }

        public static void AddApender(AppenderBuilder appenderBuilder)
        {
            AddApender(appenderBuilder.Configure());
        }

        private static void ConfigureRootLogger()
        {
            log4net.Repository.Hierarchy.Hierarchy repository = (log4net.Repository.Hierarchy.Hierarchy) LogManager.GetRepository();
            repository.Root.RemoveAllAppenders();
            repository.Root.Level = Level.All;
        }

        public static IAppender GetAppender(string appenderName) => 
            ((log4net.Repository.Hierarchy.Hierarchy) LogManager.GetRepository()).Root.GetAppender(appenderName);

        public static ILogger[] GetCurrentLoggers() => 
            ((log4net.Repository.Hierarchy.Hierarchy) LogManager.GetRepository()).GetCurrentLoggers();

        public static ILog GetLogger<T>() => 
            GetLogger(typeof(T));

        public static ILog GetLogger(object obj) => 
            GetLogger(obj.GetType());

        public static ILog GetLogger(Type t) => 
            LogManager.GetLogger(t);

        public static void Init()
        {
            ConfigureRootLogger();
        }

        public static ICollection LoadConfiguration(FileInfo fileInfo) => 
            XmlConfigurator.Configure(fileInfo);

        public static ICollection LoadConfiguration(Stream configStream) => 
            XmlConfigurator.Configure(configStream);

        public static void RemoveApender(string appenderName)
        {
            ((log4net.Repository.Hierarchy.Hierarchy) LogManager.GetRepository()).Root.RemoveAppender(appenderName);
        }
    }
}

